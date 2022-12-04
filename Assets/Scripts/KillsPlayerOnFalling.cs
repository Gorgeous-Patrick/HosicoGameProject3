using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillsPlayerOnFalling : MonoBehaviour
{
  public Rigidbody2D RB2D;  //Set reference in inspector
  public float fallVelThreshold = -0.5f;

    [SerializeField] private float toleranceVal = 0.126f;
  private bool isFalling = false;


  // Start is called before the first frame update
  void Start()
  {
    isFalling = false;
    RB2D = GetComponent<Rigidbody2D>();
        if (gameObject.GetComponent<BoxCollider2D>() == null && gameObject.GetComponent<PolygonCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
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
        float offsetLeft, offsetRight, playerOffsetLeft, playerOffsetRight, offsetBottom, playerOffsetTop;
        offsetRight = playerOffsetRight = playerOffsetTop = offsetBottom = 0;

        // check if the rubble is falling and if the hit object hit is player
        if (collision.transform.CompareTag("Player") && isFalling)
        {
            if (gameObject.GetComponent<BoxCollider2D>() != null)
            {
                // save the right position of the rubble's collider in the game world
                offsetRight = gameObject.GetComponent<BoxCollider2D>().size.x / 2;
                // save the left position of the rubble's collider in the game world
                offsetLeft = -offsetRight;

                // save the bottom position of the rubble's collider in the game world
                offsetBottom = -gameObject.GetComponent<BoxCollider2D>().size.y / 2;

                // add the offset of the collider to the center of the rubble transform
                // rubbleBorder.x += gameObject.GetComponent<BoxCollider2D>().offset.x;
                // rubbleBorder.y += gameObject.GetComponent<BoxCollider2D>().offset.y;

                // save the bottom position to the Vector2 rubbleBorder variable
                // rubbleBorder.y += offsetBottom;
            }

            if (collision.gameObject.GetComponent<CapsuleCollider2D>() != null)
            {
                // save the right position of the player's collider in the game world
                playerOffsetRight = collision.gameObject.GetComponent<CapsuleCollider2D>().size.x / 2;
                // save the left position of the player's collider in the game world
                playerOffsetLeft = -playerOffsetRight;

                // save the top position of the player's collider in the game world
                playerOffsetTop = collision.gameObject.GetComponent<CapsuleCollider2D>().size.y / 2;

                // add the offset of the collider to the center of the player transform
                // playerBorder.x += collision.gameObject.GetComponent<CapsuleCollider2D>().offset.x;
                // playerBorder.y += collision.gameObject.GetComponent<CapsuleCollider2D>().offset.y;

                // save the bottom position to the Vector2 playerBorder variable
                // playerBorder.y += playerOffsetTop;
            }

            // check if rubble is above the player (y position)
            if ((collision.transform.position.y + playerOffsetTop - transform.position.y + offsetBottom) <= 0)
            {
                // add up the size of colliders
                float distBetweenColliders = offsetRight + playerOffsetRight;

                // find distance between player transform and rubble transform
                float distBetweenTransforms = Mathf.Abs(collision.transform.position.x - transform.position.x);

                Debug.Log("killTolerance" + distBetweenColliders);
                Debug.Log("dist: " + distBetweenTransforms);

                // if the player is under the rubble (with some tolerance) -> kill player
                if (Mathf.Abs(distBetweenColliders - distBetweenTransforms) > toleranceVal)
                {
                    EventBus.Publish(new EventFailure());
                }
            }

            // Old code, remove if new code works

            /*// EventBus.Publish(new EventFailure());
             foreach (ContactPoint2D hitPos in collision.contacts)
              {
                Debug.Log(hitPos.point);
                if (hitPos.point.y - transform.position.y < 0)
                {
                  EventBus.Publish(new EventFailure());
                }
              }*/
        }
    }
}
