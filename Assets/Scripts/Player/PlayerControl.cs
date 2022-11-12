using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{
    Subscription<EventResetPlayer> sub_EventFailure;
    Subscription<EventJumpFromLadder> sub_EventJumpFromLadder;
    Rigidbody2D rb2d;
  Collider2D c2d;
  Animator anim;
  SpriteRenderer sr;
  [SerializeField] GameObject pickaxe;
  bool headlightOn;

  [SerializeField] float speed = 5f, jumpPower = 7f;

  // dir: a direction to detect collision in
  // returns: true iff. the player is next to something in the given direction
  bool colliding(Direction dir, bool ignoreDynamic = true)
  {
    if (dir == Direction.Undefined) return false;
    Vector2 box = new Vector2();
    float rayLength = 0.01f;
    switch (dir)
    {
    case Direction.Down :
    case Direction.Up:
      rayLength += c2d.bounds.extents.y;
      box.x = c2d.bounds.size.x;
      box.y = 0.1f;
      break;
    case Direction.Left :
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
    if (!ignoreDynamic) return hits.Count > 0;
    // ignore dynamic objects: go over the list of hits and return true once a non-dynamic object is found
    foreach (var hit in hits)
      if (hit.rigidbody.bodyType != RigidbodyType2D.Dynamic) return true;
    return false;
  }

  void Awake()
  {
    anim = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
    c2d = GetComponent<Collider2D>();
    sr = GetComponent<SpriteRenderer>();
        sub_EventFailure = EventBus.Subscribe<EventResetPlayer>(OnResetDo);
        sub_EventJumpFromLadder = EventBus.Subscribe<EventJumpFromLadder>(OnJumpDo);
    }

    void Start()
  {
    // pickaxe = transform.Find("Pickaxe").gameObject;
    if (pickaxe == null) Debug.LogError("Pickaxe not found");
    pickaxe.SetActive(false);
    headlightOn = true;
  }

  void Update()
  {
    bool grounded = colliding(Direction.Down, false); // can jump when standing on dynamic object
    anim.SetBool("grounded", grounded);
    anim.SetFloat("velX", rb2d.velocity.x);
    anim.SetFloat("velY", rb2d.velocity.y);

    // reset horizontal movement
    rb2d.velocity = new Vector2(0, rb2d.velocity.y);

    // process horizontal movement
    float movementInput = Gameplay.playerInput.Gameplay.Move.ReadValue<float>();
    Vector2 horizontalMovementDelta = new Vector2(movementInput * speed, 0);

    // set the animation to run and let the miner face left
    switch (movementInput)
    {
    case >0:
      sr.flipX = false;
      pickaxe.GetComponent<SwingPickaxe>().reversed = true;
      anim.SetBool("running", true);
      break;
    case <0:
      sr.flipX = true;
      pickaxe.GetComponent<SwingPickaxe>().reversed = false;
      anim.SetBool("running", true);
      break;
    case 0:
      anim.SetBool("running", false);
      break;
    }

    // prevent the player sticking to the wall/ceiling due to continuous pressure and friction
    if (!colliding(Utils.vec2dir(horizontalMovementDelta)))
      rb2d.velocity += horizontalMovementDelta;

    // process jumps
    if (Gameplay.playerInput.Gameplay.Jump.IsPressed() && grounded && rb2d.velocity.y <= 0.1f)
    {
            OnJumpDo(null);
    }

    // process mining
    if (Gameplay.playerInput.Gameplay.Mine.WasPressedThisFrame())
      pickaxe.SetActive(true);
    if (Gameplay.playerInput.Gameplay.Mine.WasReleasedThisFrame())
      pickaxe.SetActive(false);

    // process headlight toggle
    if (Gameplay.playerInput.Gameplay.ToggleHeadlight.WasPressedThisFrame())
    {
      headlightOn = !headlightOn;
      EventBus.Publish(new EventHeadlightStatusChange {enabled = headlightOn});
    }

    // process active interaction with objects
        if (Gameplay.playerInput.Gameplay.Interact.WasPressedThisFrame())
        {
            if (Gameplay.funcInteract != null)
                Gameplay.funcInteract.Invoke();
        }

  }

  IEnumerator coroutine_jumpAnim()
  {
    anim.SetBool("jumping", true);
    yield return new WaitForSeconds(0.5f);
    anim.SetBool("jumping", false);
  }

    private void OnResetDo(EventResetPlayer obj)
    {
        transform.position = obj.pos;
    }

    private void OnJumpDo(EventJumpFromLadder obj)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.velocity += new Vector2(0, jumpPower);
        StartCoroutine(coroutine_jumpAnim());
    }

}
