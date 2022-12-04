using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] bool isInTutorial = false;

    [SerializeField] GameObject ropeInContact; // null if not touching any rope
    int touchingLadderCnt;
    bool touchingLadder
    {
        get => touchingLadderCnt > 0;
    }
    bool touchingRope
    {
        get => ropeInContact != null;
    }

    bool _flying; // just jumped off ladder or rope?
    bool flying
    {
        get => _flying;
        set
        {
            _flying = value;
            if (value == false) {
                if (ropeInContact != null && touchingLadder) {
                    if (lastClimbStatus == ClimbStatus.Ladder)
                        attachToRope();
                    else if (lastClimbStatus == ClimbStatus.Rope)
                        climb = ClimbStatus.Ladder;
                }
                else if (ropeInContact != null)
                    attachToRope();
                else if (touchingLadder)
                    climb = ClimbStatus.Ladder;
            }
        }
    }
    bool ropeClimbCooling;

    [SerializeField] ClimbStatus _climb;
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

    ClimbStatus lastClimbStatus;
    float gravityScale;

    [SerializeField] float speed = 5f, jumpPower = 7f, climbSpeed = 3f;

    // dir: a direction to detect collision in
    // returns: true iff. the player is next to something in the given direction
    bool colliding(Direction dir, bool ignoreDynamic = true) => colliding(dir, transform.position, ignoreDynamic);
    bool colliding(Direction dir, Vector2 origin, bool ignoreDynamic = true) {
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
        Physics2D.BoxCast(origin, box, 0, Utils.dir2vec(dir), filter, hits, rayLength);
        // we do not ignore dynamic objects, so as long as there is any collision we return true
        // foreach (var hit in hits)
        //   if (hit.transform.CompareTag("FallingRubble") /*|| (hit.transform.CompareTag("PushableBlock"))*/) return true;
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
        EventBus.Subscribe<EventReset>((e) => StartCoroutine(coroutine_Death(e)));
    }

    void Start() {
        pickaxe = transform.Find("Pickaxe Container").Find("Pickaxe").gameObject;
        if (pickaxe == null) Debug.LogError("Pickaxe not found");
        pickaxe.SetActive(false);
        headlightOn = false;
        gravityScale = rb2d.gravityScale;

        hinge = GetComponent<FixedJoint2D>();
        hinge.enabled = false;

        flying = false;
        ropeClimbCooling = false;
        climb = ClimbStatus.None;
        touchingLadderCnt = 0;
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
        // animation
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
            rb2d.velocity += horizontalMovementDelta;

        // process ladder climbing
        float climbInput = Gameplay.playerInput.Gameplay.Climb.ReadValue<float>();
        // allow player to re-grab the ladder
        if (climbInput != 0 && flying && (touchingLadder || touchingRope)) flying = false;
        Vector2 verticalMovementDelta = new Vector2(0, 0);
        if (climb == ClimbStatus.Ladder) {
            verticalMovementDelta = new Vector2(0, climbInput * climbSpeed);
        }
        // apply vertical movement
        rb2d.velocity += verticalMovementDelta;

        // process rope climbing
        if (climb == ClimbStatus.Rope && climbInput != 0) {
            GameObject target = traverseRope(hinge.connectedBody.gameObject, Math.Sign(climbInput) * 5);
            if (!ropeClimbCooling)
                StartCoroutine(coroutine_climbRope(target));
        }

        // process jumps
        if (Gameplay.playerInput.Gameplay.Jump.IsPressed()
            && ((grounded && rb2d.velocity.y <= 0.1f) || climb != ClimbStatus.None)) {
            if (climb != ClimbStatus.None)
                StartCoroutine(coroutine_jumpOff());
            // fix jumping on spikes issue - pull the player down a little bit so that they touch the spikes
            transform.position += new Vector3(0, -0.02f);
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
                touchingLadderCnt++;
            else if (c.type == ClimbStatus.Rope)
                ropeInContact = collisionInfo.gameObject;

            if (!flying && touchingLadder && climb != ClimbStatus.Ladder) climb = ClimbStatus.Ladder;
            if (!flying && c.type == ClimbStatus.Rope && climb != ClimbStatus.Rope) attachToRope();
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
                flying = false;
                touchingLadderCnt--;
                if (!touchingLadder && !touchingRope)
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
        lastClimbStatus = climb;
        detachFromRope();
        yield return new WaitForSeconds(1f);
        flying = false;
    }

    IEnumerator coroutine_climbRope(GameObject next) {
        ropeClimbCooling = true;
        hinge.connectedBody = next.GetComponent<Rigidbody2D>();
        float starttime = Time.time;
        Vector2 startpos = transform.position;
        float progress = 0f;
        while (progress < 1.0f) {
            progress = (Time.time - starttime) / 0.15f;
            transform.position = Vector2.Lerp(startpos, hinge.connectedBody.transform.position, progress);
            yield return null;
        }
        transform.position = hinge.connectedBody.transform.position;
        ropeClimbCooling = false;
    }

    void attachToRope() {
        climb = ClimbStatus.Rope;
        hinge.connectedBody = ropeInContact.GetComponent<Rigidbody2D>();
        transform.position = ropeInContact.transform.position;
        hinge.enabled = true;
    }

    void detachFromRope() {
        climb = ClimbStatus.None;
        hinge.enabled = false;
    }

    GameObject traverseRope(GameObject seg, int cnt) {
        GameObject ret = seg;
        while (cnt != 0) {
            GameObject next = cnt > 0 ? ret.GetComponent<RopeSegment>().prev : ret.GetComponent<RopeSegment>().next;
            if (next == null || colliding(Direction.Up, next.transform.position, false)) return ret;
            ret = next;
            cnt = cnt > 0 ? cnt - 1 : cnt + 1;
        }
        return ret;
    }

    // plays out death sequence
    IEnumerator coroutine_Death(EventReset e) {
        // play death grunt
        AudioManager.instance.playSound("4-player_death", 1.0f);

        // disable player movement
        Gameplay.playerInput.Gameplay.Disable();
        var original_constraints = rb2d.constraints;
        rb2d.constraints |= RigidbodyConstraints2D.FreezePositionX;
        pickaxe.SetActive(false);
        rb2d.constraints |= RigidbodyConstraints2D.FreezePositionY;

        // disable battery ui
        this.gameObject.transform.GetChild(3).gameObject.SetActive(false);

        // WIP
        // Spotlight on player
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<Light2D>().color = Color.white;
        // zoom camera on player
        // NOTE: requires perspective (vertical) camera
        CinemachineFramingTransposer CineCamera = Camera.main.transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        if (CineCamera != null) {
            CineCamera.m_CameraDistance = 5.0f;
        }

        // play player death animation
        anim.SetBool("dead", true);
        yield return new WaitForSeconds(2.5f);

        // call and wait for fade out transition to play out
        EventBus.Publish(new EventStartTransition { isStart = true });
        yield return new WaitForSeconds(1.75f);

        if (e.resetEntireLevel) {
            // fade in UI element
            EventBus.Publish(new EventShowHealthUI { isStart = true, isRed = true });
            yield return new WaitForSeconds(2.0f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetString("currScene", SceneManager.GetActiveScene().name);
        }
        else {
            anim.SetBool("dead", false);
            transform.position = CheckpointController.checkpoint;

            // reactivate battery ui
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);

            // WIP
            // Remove spotlight on player
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Light2D>().color = new Vector4(0.15f, 0.15f, 0.15f, 1.0f);
            // un-zoom camera on player
            // NOTE: requires perspective (vertical) camera
            if (CineCamera != null) {
                CineCamera.m_CameraDistance = 10.0f;
            }

            // fade in UI element
            EventBus.Publish(new EventShowHealthUI { isStart = true });
            yield return new WaitForSeconds(2.0f);
            // fade out UI element
            EventBus.Publish(new EventShowHealthUI { isStart = false });
            yield return new WaitForSeconds(1.0f);

            // call and wait for fade in transition
            EventBus.Publish(new EventStartTransition { isStart = false });
            yield return new WaitForSeconds(1.75f);

            // enable player movement
            Gameplay.playerInput.Gameplay.Enable();
            rb2d.constraints = original_constraints;
        }
        EventBus.Publish(new EventToggleInvincibility { invincible = false });
    }
}
