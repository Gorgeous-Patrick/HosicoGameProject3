using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{

  Rigidbody2D rb2d;
  Collider2D c2d;
  Animator anim;
  SpriteRenderer sr;
  [SerializeField] float speed = 5f, jumpPower = 7f;

  // dir: a direction to detect collision in
  // returns: true iff. the player is next to something in the given direction
  bool colliding(Direction dir)
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
    return Physics2D.BoxCast(transform.position, box, 0, Utils.dir2vec(dir), filter, new List<RaycastHit2D>(), rayLength) > 0;
  }

  void Awake()
  {
    anim = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
    c2d = GetComponent<Collider2D>();
    sr = GetComponent<SpriteRenderer>();
  }

  void Update()
  {
    bool grounded = colliding(Direction.Down);
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
      anim.SetBool("running", true);
      break;
    case <0:
      sr.flipX = true;
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
      rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
      rb2d.velocity += new Vector2(0, jumpPower);
      StartCoroutine(coroutine_jumpAnim());
    }
  }

  IEnumerator coroutine_jumpAnim()
  {
    anim.SetBool("jumping", true);
    yield return new WaitForSeconds(0.5f);
    anim.SetBool("jumping", false);
  }

}

