using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillsPlayerOnFalling : MonoBehaviour
{
  public Rigidbody2D RB2D;  //Set reference in inspector
  public float fallVelThreshold = -0.5f;
  private bool isFalling = false;

  // Start is called before the first frame update
  void Start()
  {
    isFalling = false;
    RB2D = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (RB2D.velocity.y < fallVelThreshold)
      isFalling = true;
    else isFalling = false;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.transform.CompareTag("Player") && isFalling)
    {
      // EventBus.Publish(new EventFailure());
      foreach (ContactPoint2D hitPos in collision.contacts)
      {
        Debug.Log(hitPos.point);
        if (hitPos.point.y - transform.position.y < 0)
        {
          EventBus.Publish(new EventFailure());
        }
      }
    }
  }
}
