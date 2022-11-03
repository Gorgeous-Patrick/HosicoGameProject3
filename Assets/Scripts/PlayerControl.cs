using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
  public UnityEvent<Vector2> OnRotateFlashLight = new UnityEvent<Vector2>();
  public UnityEvent<Vector2> OnMovePlayerCharacter = new UnityEvent<Vector2>();
  public UnityEvent OnMine = new UnityEvent();
  public UnityEvent OnSwapItem = new UnityEvent();
  public UnityEvent OnChangeForm = new UnityEvent();
  public float speed = 5;

  private Camera playerCam;
  private bool isBirb = false;
  private Rigidbody2D rb2d;
  // private CircleCollider2D collider;
  // private float distToGround;
  // private bool  is_grounded = false;
  private bool isGrounded
  {
    get
    {
      Debug.Log(new Vector2(rb2d.transform.position.x, rb2d.transform.position.y));
      Debug.Log(GetComponent<CircleCollider2D>().bounds.extents.y);
      return Physics2D.Raycast(new Vector2(rb2d.transform.position.x, rb2d.transform.position.y), Vector2.down, GetComponent<CircleCollider2D>().bounds.extents.y + 0.1f);
    }
  }

  void Start()
  {
    playerCam = Camera.main;
    rb2d = gameObject.GetComponent<Rigidbody2D>();
    Debug.Log("start");
    // collider = gameObject.GetComponent<CircleCollider2D>();
  }

  void Update()
  {
    // get movement vectors: horizontal, vertical
    // distToGround = collider.bounds.extents.y;
    float x = Input.GetAxis("Horizontal");
    float y = Input.GetAxis("Vertical");
    Vector2 moveVec = new Vector2(x, y);
    rb2d.velocity = new Vector2(x * speed, rb2d.velocity.y);
    // Debug.Log(x);
    // call OnMoveHullEvent and invoke it passing the movement Vector
    OnMovePlayerCharacter?.Invoke(moveVec.normalized);

    //==============================================================================//
    /*
    Get mouse position in 3D; then get main camera;
    then convert to 2D using nearClipPlane and ScreenToWOrldPoint
    */
    if (playerCam == null)
    {
      Debug.Log("Camera is null; something's wrong");
      return;
    }
    Vector3 MPos = Input.mousePosition;
    MPos.z = playerCam.nearClipPlane;
    Vector2 MPos2D = playerCam.ScreenToWorldPoint(MPos);
    OnRotateFlashLight?.Invoke(MPos2D);

    //==============================================================================//
    if (Input.GetMouseButtonDown(0))
    {
      OnMine?.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      OnSwapItem?.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.G))
    {
      EventBus.Publish(new EventDidPopSmoke());
    }
    if (Input.GetKeyDown(KeyCode.F))
    {
      EventBus.Publish(new EventStopTank());
      OnChangeForm?.Invoke();
    }
    if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
    {
      rb2d.velocity = new Vector2(rb2d.velocity.x, 10f);
    }
    // Debug.Log(distToGround);
    // Debug.Log(IsGrounded);
    // Debug.Log(is_grounded);
    // OnCollisionStay();
  }


  // private void OnCollisionStay(Collision collisionInfo)
  // {
  //     Debug.Log(collisionInfo.gameObject.tag);
  //     Debug.Log("collision");
  //     if (collisionInfo.gameObject.CompareTag("Ground")) {
  //         is_grounded = true;
  //     }
  //     else {
  //         is_grounded = false;
  //     }
  // }
  // private void OnCollisionEnter(Collision collisionInfo)
  // {
  //     Debug.Log(collisionInfo.gameObject.tag);
  //     Debug.Log("collisionenter");
  //     if (collisionInfo.gameObject.CompareTag("Ground")) {
  //         is_grounded = true;
  //     }
  //     else {
  //         is_grounded = false;
  //     }
  // }
}
