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
    [SerializeField] CircleCollider2D blastRadius;

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
    Collider2D[] overCollider = Physics2D.OverlapCircleAll(pickPoint.position, pickaxeHitBoxSize, WhatIsPlatform);
    foreach (var hit in overCollider)
        {
            if (hit != null)
            {
                AudioManager.instance.playSound("3-dig_rocks", 1.0f);
                hit.transform.GetComponent<MinableTile>().DestroyTileMapAtPoint(pickPoint.position);
            }
        }
  }

    private void Start()
    {
        StartCoroutine(ExplosionActivated());
    }

    private IEnumerator ExplosionActivated()
    {
        yield return new WaitForSeconds(0.2f);
        DestroyTilesArea();
    }

    private void DestroyTilesArea()
    {
        int radius = Mathf.RoundToInt(blastRadius.radius);
        for (var x = -radius; x <= radius; x++)
        {
            for (var y = -radius; y < radius; y++)
            {
                Vector3 tilePos = new Vector3(transform.position.x + x, transform.position.y + y, 0);
                float dist = Vector2.Distance(transform.position, tilePos) - 0.001f;

                if (dist <= radius)
                {
                    Collider2D overCollider2d = Physics2D.OverlapCircle(tilePos, 0.01f, WhatIsPlatform);
                    if (overCollider2d != null)
                    {
                        overCollider2d.transform.GetComponent<MinableTile>().DestroyTileMapAtPoint(tilePos);
                    }
                }
            }
        }
    }
}
