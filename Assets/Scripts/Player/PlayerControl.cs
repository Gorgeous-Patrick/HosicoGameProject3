using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FixedJoint2D))]
public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb2d;
    Collider2D c2d;
    Animator anim;
    SpriteRenderer sr;
    FixedJoint2D hinge;

    GameObject pickaxe;
    bool headlightOn;
    bool canMove = true;
    [SerializeField] bool isInTutorial = false;

    Rigidbody2D ropeInContact;
    bool touchingLadder;
    bool flying;
    ClimbStatus _climb;
    ClimbStatus climb
    {
        get => _climb;
        set
        {
            rb2d.gravityScale = (value == ClimbStatus.Ladder) ? 0 : gravityScale;
            _climb = value;
            anim.SetBool("climbing", value != ClimbStatus.None);
        }
    }
    float gravityScale;

    [SerializeField] float speed = 5f, jumpPower = 7f, climbSpeed = 3f;

    // dir: a direction to detect collision in
    // returns: true iff. the player is next to something in the given direction
    bool colliding(Direction dir, bool ignoreDynamic = true) {
        if (dir == Direction.Undefined) return false;
        Vector2 box = new Vector2();
        float rayLength = 0.01f;
        switch (dir) {
            case Direction.Down:
            case Direction.Up:
                rayLength += c2d.bounds.extents.y;
                box.x = c2d.bounds.size.x;
                box.y = 0.1f;
                break;
            case Direction.Left:
            case Direction.Right:
                rayLength += c2d.bounds.extents.x;
                box.x = 0.1f;
                box.y = c2d.bounds.size.y;
                break;
        }
        var filter = new ContactFilter2D();
        filter.useTriggers = false;
        var hits = new List<RaycastHit2D>();
        Physics2D.BoxCast(transform.position, box, 0, Utils.dir2vec(dir), filter, hits, rayLength);
        // we do not ignore dynamic objects, so as long as there is any collision we return true
        foreach (var hit in hits)
            if (hit.transform.CompareTag("FallingRubble")) return true;
        if (!ignoreDynamic) return hits.Count > 0;
        // ignore dynamic objects: go over the list of hits and return true once a non-dynamic object is found
        foreach (var hit in hits)
            if (hit.rigidbody.bodyType != RigidbodyType2D.Dynamic) return true;
        return false;
    }

    void Awake() {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        EventBus.Subscribe<EventResetPlayer>(handler_EventResetPlayer);
    }

    void Start() {
        if (!canMove) { return; };

        pickaxe = transform.Find("Pickaxe Container").Find("Pickaxe").gameObject;
        if (pickaxe == null) Debug.LogError("Pickaxe not found");
        pickaxe.SetActive(false);
        headlightOn = true;
        gravityScale = rb2d.gravityScale;

        // Init a hinge joint so that it can be used to climb. Disabled by default.
        hinge = GetComponent<FixedJoint2D>();
        hinge.enabled = false;

        flying = false;
        climb = ClimbStatus.None;
        touchingLadder = false;
        ropeInContact = null;
    }

    void Update() {
        bool grounded = colliding(Direction.Down, false); // can jump when standing on dynamic object
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velX", rb2d.velocity.x);
        anim.SetFloat("velY", rb2d.velocity.y);

        // reset movement
        rb2d.velocity = new Vector2(0, climb == ClimbStatus.Ladder ? 0 : rb2d.velocity.y);

        // process horizontal movement
        float movementInput = Gameplay.playerInput.Gameplay.Move.ReadValue<float>();
        Vector2 horizontalMovementDelta = new Vector2(movementInput * speed, 0);

        Vector2 verticalMovementDelta = new Vector2(0, 0);
        if (climb == ClimbStatus.Ladder) {
            float climbInput = Gameplay.playerInput.Gameplay.Climb.ReadValue<float>();
            verticalMovementDelta = new Vector2(0, climbInput * climbSpeed);
        }

        // set the animation to run and let the miner face left
        switch (movementInput) {
            case > 0:
                sr.flipX = false;
                anim.SetBool("running", true);
                break;
            case < 0:
                sr.flipX = true;
                anim.SetBool("running", true);
                break;
            case 0:
                anim.SetBool("running", false);
                break;
        }

        // prevent the player sticking to the wall/ceiling due to continuous pressure and friction
        if (!colliding(Utils.vec2dir(horizontalMovementDelta)))
            // apply horizontal movement
            // rb2d.AddForce(horizontalMovementDelta * 500);
            rb2d.velocity += horizontalMovementDelta;
        // apply vertical movement
        rb2d.velocity += verticalMovementDelta;

        // process jumps
        if (Gameplay.playerInput.Gameplay.Jump.IsPressed()
            && ((grounded && rb2d.velocity.y <= 0.1f) || climb != ClimbStatus.None)) {
            if (climb != ClimbStatus.None)
                StartCoroutine(coroutine_jumpOff());
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.velocity += new Vector2(0, jumpPower);
            StartCoroutine(coroutine_jumpAnim());
        }

        // process mining
        if (Gameplay.playerInput.Gameplay.Mine.WasPressedThisFrame() && !isInTutorial)
            pickaxe.SetActive(true);
        if (Gameplay.playerInput.Gameplay.Mine.WasReleasedThisFrame())
            pickaxe.SetActive(false);

        // process item use
        if (Gameplay.playerInput.Gameplay.UseItem.WasPressedThisFrame())
            EventBus.Publish(new EventUseItem());

        // process headlight toggle
        if (Gameplay.playerInput.Gameplay.ToggleHeadlight.WasPressedThisFrame()) {
            headlightOn = !headlightOn;
            EventBus.Publish(new EventHeadlightStatusChange { enabled = headlightOn });
        }

        // process active interaction with objects
        if (Gameplay.playerInput.Gameplay.Interact.WasPressedThisFrame()) {
            if (Gameplay.funcInteract != null)
                Gameplay.funcInteract.Invoke();
        }

    }

    void OnTriggerEnter2D(Collider2D collisionInfo) {
        Climbable c = collisionInfo.gameObject.GetComponent<Climbable>();
        if (c != null) {
            if (c.type == ClimbStatus.Ladder)
                touchingLadder = true;
            else if (c.type == ClimbStatus.Rope)
                ropeInContact = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
            if (!flying) {
                print(c.type);
                climb = c.type;
                print(climb);
                if (climb == ClimbStatus.Rope)
                    attach2Rope();
            }
        }

        if (collisionInfo.gameObject.CompareTag("PickaxePickup")) {
            isInTutorial = false;
            collisionInfo.gameObject.GetComponent<DisableAfterSeconds>().StartDisable();
        }
    }

    void OnTriggerExit2D(Collider2D collisionInfo) {
        Climbable c = collisionInfo.gameObject.GetComponent<Climbable>();
        if (c != null) {
            if (c.type == ClimbStatus.Ladder) {
                touchingLadder = false;
                climb = ClimbStatus.None;
            }
            else if (c.type == ClimbStatus.Rope)
                ropeInContact = null;
        }
    }

    IEnumerator coroutine_jumpAnim() {
        anim.SetBool("jumping", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("jumping", false);
    }

    IEnumerator coroutine_jumpOff() {
        flying = true;
        ClimbStatus s = climb;
        detachFromRope();
        yield return new WaitForSeconds(1f);
        flying = false;

        if (ropeInContact != null && touchingLadder) {
            if (s == ClimbStatus.Ladder)
                attach2Rope();
            else if (s == ClimbStatus.Rope)
                climb = ClimbStatus.Ladder;
        }
        else if (ropeInContact != null)
            attach2Rope();
        else if (touchingLadder)
            climb = ClimbStatus.Ladder;
    }

    void attach2Rope() {
        climb = ClimbStatus.Rope;
        hinge.connectedBody = ropeInContact;
        hinge.enabled = true;
    }

    void detachFromRope() {
        climb = ClimbStatus.None;
        hinge.enabled = false;
    }

    void handler_EventResetPlayer(EventResetPlayer e) {
        StartCoroutine(DeathSequence(e.pos));
    }

    IEnumerator DeathSequence(Vector2 _pos) {
        // start death animation and prevent player movement
        anim.SetBool("dead", true);
        canMove = false;
        yield return new WaitForSeconds(4.0f);

        // reset position and re-enable movement
        transform.position = _pos;
        anim.SetBool("dead", false);
        canMove = true;
    }
}
