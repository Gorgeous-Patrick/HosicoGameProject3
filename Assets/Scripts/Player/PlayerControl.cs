using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

[RequireComponent(typeof(FixedJoint2D))]
public class PlayerControl : MonoBehaviour
{
    public UnityEvent OnPlayerDeathPlay = new UnityEvent();

    Rigidbody2D rb2d;
    Collider2D c2d;
    Animator anim;
    SpriteRenderer sr;
    FixedJoint2D hinge;

    GameObject pickaxe;

    bool headlightOn;
    [SerializeField] bool isInTutorial = false;
    // [SerializeField] AudioClip deathSound;

    List<GameObject> ropeInContact = new List<GameObject>();
    List<GameObject> ladderInContact = new List<GameObject>();

    bool touchingLadder
    {
        get => ladderInContact.Count > 0;
    }
    bool touchingRope
    {
        get => ropeInContact.Count > 0;
    }

    [SerializeField] bool _flying; // just jumped off ladder or rope?
    bool flying
    {
        get => _flying;
        set
        {
            _flying = value;
            if (value == false) {
                if (touchingRope && touchingLadder) {
                    if (lastClimb.type == ClimbType.Ladder)
                        attachToRope();
                    else if (lastClimb.type == ClimbType.Rope)
                        attachToLadder();
                }
                else if (touchingRope)
                    attachToRope();
                else if (touchingLadder)
                    attachToLadder();
            }
        }
    }
    ClimbObject lastClimb;

    bool _ropeClimbCooling;
    bool ropeClimbCooling
    {
        get => _ropeClimbCooling;
        set
        {
            _ropeClimbCooling = value;
            anim.SetBool("climbingRope", value);
        }
    }

    [SerializeField] ClimbObject _climb;
    ClimbType climb
    {
        get => _climb.type;
    }
    void setClimb(GameObject o) {
        if (o == null) _climb.clear();
        else
            _climb.setTo(o);
        anim.SetBool("climbing", climb != ClimbType.None);
        rb2d.gravityScale = (climb == ClimbType.Ladder) ? 0 : gravityScale;
    }

    float gravityScale;
    bool justStarted = true;

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
        headlightOn = false;
        gravityScale = rb2d.gravityScale;
        hinge = GetComponent<FixedJoint2D>();
        reset();
    }

    void reset() {
        pickaxe.SetActive(false);
        hinge.enabled = false;
        flying = false;
        ropeClimbCooling = false;
        _climb = new ClimbObject { type = ClimbType.None, obj = null };
        GetComponent<InhalesPoisonousGas>().reset();
        StartCoroutine(StartRoutine());
    }

    void Update() {

        bool grounded = colliding(Direction.Down, false); // can jump when standing on dynamic object
        anim.SetBool("grounded", grounded);
        anim.SetFloat("velX", rb2d.velocity.x);
        anim.SetFloat("velY", rb2d.velocity.y);

        // WIP
        // process player-initiated reset
        if (Gameplay.playerInput.Gameplay.Suicide.WasReleasedThisFrame()) {
            EventBus.Publish(new EventUpdateHealth { newHealth = 0 });
            EventBus.Publish(new EventReset { isSuicide = true });
        }

        // reset movement
        rb2d.velocity = new Vector2(0, climb == ClimbType.Ladder ? 0 : rb2d.velocity.y);

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
        if (climb == ClimbType.Ladder) {
            verticalMovementDelta = new Vector2(0, climbInput * climbSpeed);
        }
        // apply vertical movement
        rb2d.velocity += verticalMovementDelta;

        // process rope climbing
        if (climb == ClimbType.Rope && climbInput != 0) {
            GameObject target = traverseRope(hinge.connectedBody.gameObject, Math.Sign(climbInput) * 5);
            if (!ropeClimbCooling)
                StartCoroutine(coroutine_climbRope(target));
        }

        // process jumps
        if (Gameplay.playerInput.Gameplay.Jump.IsPressed()
            && ((grounded && rb2d.velocity.y <= 0.1f) || climb != ClimbType.None)) {
            if (climb != ClimbType.None)
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
        if (Gameplay.playerInput.Gameplay.UseHeadlight.WasPressedThisFrame()) {
            if (Gameplay.batterys > 0) {
                headlightOn = !headlightOn;
                EventBus.Publish(new EventHeadlightStatusChange { enabled = true });
            }
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
            if (c.type == ClimbType.Ladder)
                ladderInContact.Add(collisionInfo.gameObject);
            else if (c.type == ClimbType.Rope)
                ropeInContact.Add(collisionInfo.gameObject);

            if (!flying || !lastClimb.matches(collisionInfo.gameObject)) {
                if (c.type == ClimbType.Ladder && climb != ClimbType.Ladder) {
                    detachFromClimbables();
                    attachToLadder();
                }
                if (c.type == ClimbType.Rope && climb != ClimbType.Rope) {
                    attachToRope();
                }
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
            if (c.type == ClimbType.Ladder)
                ladderInContact.Remove(collisionInfo.gameObject);
            else if (c.type == ClimbType.Rope)
                ropeInContact.Remove(collisionInfo.gameObject);
            if (!touchingLadder && !touchingRope)
                setClimb(null);
        }
    }

    IEnumerator coroutine_jumpAnim() {
        anim.SetBool("jumping", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("jumping", false);
    }

    IEnumerator coroutine_jumpOff() {
        flying = true;
        lastClimb = _climb;
        detachFromClimbables();
        yield return new WaitForSeconds(0.5f);
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

    void attachToLadder() {
        GameObject ladder = Utils.RandomChoice(ladderInContact);
        if (_climb.matches(ladder)) return;
        setClimb(ladder);
    }

    void attachToRope() {
        GameObject seg = Utils.RandomChoice(ropeInContact);
        if (_climb.matches(seg)) return; // don't attach to the same rope
        setClimb(seg);
        hinge.connectedBody = seg.GetComponent<Rigidbody2D>();
        transform.position = seg.transform.position;
        hinge.enabled = true;
    }

    void detachFromClimbables() {
        setClimb(null);
        hinge.enabled = false;
    }

    GameObject traverseRope(GameObject seg, int cnt) {
        GameObject ret = seg;
        while (cnt != 0) {
            GameObject next = cnt > 0 ? ret.GetComponent<RopeSegment>().prev : ret.GetComponent<RopeSegment>().next;
            if (next == null || (cnt > 0 && colliding(Direction.Up, next.transform.position, false))) return ret;
            ret = next;
            cnt = cnt > 0 ? cnt - 1 : cnt + 1;
        }
        return ret;
    }

    // plays out death sequence
    IEnumerator coroutine_Death(EventReset e) {
        GameObject batteryCanvas = GameObject.Find("Battery Canvas");
        GameObject ambientLight = GameObject.Find("Ambient Light");
        CinemachineFramingTransposer CineCamera = Camera.main.transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        RigidbodyConstraints2D original_constraints = rb2d.constraints;

        if (!e.isSuicide) {
            // play death grunt
            //udioManager.instance.playSound("4-player_death", 10.0f);
            /*if (deathSound != null)
            {
                        // AudioSource.PlayClipAtPoint(deathSound, transform.position, 10.0f);
                        OnPlayerDeathPlay?.Invoke();
            } */
            OnPlayerDeathPlay?.Invoke();

            // disable player movement
            Gameplay.playerInput.Gameplay.Disable();
            pickaxe.SetActive(false);
            rb2d.constraints |= RigidbodyConstraints2D.FreezePositionX;
            rb2d.constraints |= RigidbodyConstraints2D.FreezePositionY;

            // disable battery ui
            if (batteryCanvas != null) {
                batteryCanvas.SetActive(false);
            }

            if (!e.noZoomIn) {
                // Spotlight on player
                ambientLight.GetComponent<Light2D>().color = Color.white;
                // zoom camera on player
                // NOTE: requires perspective (vertical) camera
                CineCamera.m_CameraDistance = 5.0f;
            }

            // play player death animation
            anim.SetBool("dead", true);
            yield return new WaitForSeconds(2.5f);
            // OnPlayerRespawnStop?.Invoke();
        }

        // call and wait for fade out transition to play out
        EventBus.Publish(new EventStartTransition { isStart = true });
        yield return new WaitForSeconds(1.75f);

        if (e.resetEntireLevel || e.isSuicide) {
            // fade in UI element
            EventBus.Publish(new EventShowHealthUI { isStart = true, isZero = e.resetEntireLevel, isSuicide = e.isSuicide });
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetString("currScene", SceneManager.GetActiveScene().name);
        }
        else {
            anim.SetBool("dead", false);

            // reactivate battery ui
            // disable battery ui
            if (batteryCanvas != null) {
                batteryCanvas.SetActive(true);
            }

            if (!e.noZoomIn) {
                // Remove spotlight on player
                ambientLight.GetComponent<Light2D>().color = new Vector4(0.15f, 0.15f, 0.15f, 1.0f);
                // un-zoom camera on player
                // NOTE: requires perspective (vertical) camera
                CineCamera.m_CameraDistance = 10.0f;
            }

            // fade in UI element
            EventBus.Publish(new EventShowHealthUI { isStart = true });
            yield return new WaitForSeconds(2.0f);
            // fade out UI element
            EventBus.Publish(new EventShowHealthUI { isStart = false });
            yield return new WaitForSeconds(1.0f);

            // reset stuff
            rb2d.constraints = original_constraints;
            transform.position = CheckpointController.checkpoint;
            reset();

            // call and wait for fade in transition
            EventBus.Publish(new EventStartTransition { isStart = false });
            yield return new WaitForSeconds(1.75f);


            // enable player movement
            Gameplay.playerInput.Gameplay.Enable();
        }
        EventBus.Publish(new EventToggleInvincibility { invincible = false });
    }

    IEnumerator StartRoutine() {
        yield return new WaitForSeconds(0.2f);
        justStarted = false;
    }
}
