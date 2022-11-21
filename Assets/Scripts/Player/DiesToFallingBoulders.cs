using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiesToFallingBoulders : MonoBehaviour
{

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Boulder")
    {
      Rigidbody2D BoulderRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
      if (BoulderRigidbody == null) return;
      if (BoulderRigidbody.velocity.y < 0 && collision.GetContact(0).point.y > (transform.position.y + 0.45))
        EventBus.Publish(new EventFailure());
    }
  }

}
