using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasPlayerClimbScript : MonoBehaviour
{
    private float vertical;
    private bool isObjLadder;
    private bool isOnLadder;

    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float spd;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if (isObjLadder && Mathf.Abs(vertical) > 0f)
            isOnLadder = true;
    }

    private void FixedUpdate()
    {
        if (isOnLadder)
        {
            rb2d.gravityScale = 0f;
            rb2d.velocity = new Vector2(rb2d.velocity.x, vertical * spd);
        }
        else rb2d.gravityScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Climbable"))
            isObjLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climbable"))
        {
            isObjLadder = false;
            isOnLadder = isObjLadder;
        }
    }
}
