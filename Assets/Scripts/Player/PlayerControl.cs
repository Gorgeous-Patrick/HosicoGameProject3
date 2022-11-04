using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControl : MonoBehaviour
{

  Rigidbody2D rb2d;
  Collider2D c2d;
  Animator anim;
  [SerializeField] float speed = 5f, jumpPower = 3.5f;
  [SerializeField] GameObject flashlight;

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
    return Physics2D.BoxCast(transform.position, box, 0, Utils.dir2vec(dir), rayLength);
  }

  void Awake()
  {
    anim = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
    c2d = GetComponent<Collider2D>();
  }

  void OnEnable()
  {
    Gameplay.playerInput.Gameplay.Enable();
  }

  void OnDisable()
  {
    Gameplay.playerInput.Gameplay.Disable();
  }

  void Update()
  {
    // reset horizontal movement
    rb2d.velocity = new Vector2(0, rb2d.velocity.y);

    // process horizontal movement
    float movementInput = Gameplay.playerInput.Gameplay.Move.ReadValue<float>();
    Vector2 horizontalMovementDelta = new Vector2(movementInput * speed, 0);

    // set the animation to run and let the miner face left
    if (movementInput > 0) {
      anim.SetTrigger("Run");
      anim.ResetTrigger("NotRun");
    }
    // set the animation to run and let the miner face right
    else if (movementInput < 0) {
      anim.SetTrigger("Run");
      anim.ResetTrigger("NotRun");
    }
    // set the animation to notrun and let the miner face left
    else {
      anim.SetTrigger("NotRun");
      anim.ResetTrigger("Run");
    }

    // prevent the player sticking to the wall/ceiling due to continuous pressure and friction
    if (!colliding(Utils.vec2dir(horizontalMovementDelta)))
      rb2d.velocity += horizontalMovementDelta;

    // process jumps
    if (Gameplay.playerInput.Gameplay.Jump.IsPressed() && colliding(Direction.Down) && rb2d.velocity.y <= 0.1f)
    {
      rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
      rb2d.velocity += new Vector2(0, jumpPower);
      // set to jump animation
      anim.SetTrigger("Jump");
      anim.ResetTrigger("NotJump");
    }
    else if (!Gameplay.playerInput.Gameplay.Jump.IsPressed() && colliding(Direction.Down) && rb2d.velocity.y <= 0.1f) {
      // set back to idle animation
      anim.SetTrigger("NotJump");
      anim.ResetTrigger("Jump");
    }
  }

}


