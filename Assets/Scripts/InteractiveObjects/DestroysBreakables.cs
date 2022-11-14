using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroysBreakables : MonoBehaviour
{
  public LayerMask WhatIsPlatform;

  public int durabilityImpact = -1;
    public bool isExplosion = false;
  [SerializeField] Transform pickPoint;
  [SerializeField] float pickaxeHitBoxSize = 0.1f;

  private void Awake()
  {
    if (pickPoint == null)
    {
      pickPoint = transform;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    breakTileHelper();
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    breakTileHelper();
  }

  private void breakTileHelper()
  {
    Collider2D overCollider = Physics2D.OverlapCircle(pickPoint.position, pickaxeHitBoxSize, WhatIsPlatform);
    if (overCollider != null)
    {
      overCollider.transform.GetComponent<MinableTile>().DestroyTileMapAtPoint(pickPoint.position);
    }
  }


  /*if (collision.gameObject.CompareTag("BreakableGround"))
  {

      Vector2 hitPos = transform.position;
      hitPos.x = Mathf.Floor(hitPos.x);
      hitPos.y = Mathf.Floor(hitPos.y);
      Debug.Log("hit breakable ground at " + hitPos);
      collision.gameObject.GetComponent<HasCanBeDugScript>().DestroyTileMapAtPoint(hitPos);
  }*/
}
