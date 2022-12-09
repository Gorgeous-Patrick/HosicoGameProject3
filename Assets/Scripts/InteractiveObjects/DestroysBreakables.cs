using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroysBreakables : MonoBehaviour
{
    public LayerMask WhatIsPlatform;

    public int durabilityImpact = -1;
    public bool isExplosion = false;

    [SerializeField] float mineInterval;
    float cooldown;

    [SerializeField] Transform pickPoint;
    [SerializeField] float pickaxeHitBoxSize = 0.1f;
    [SerializeField] CircleCollider2D blastRadius;
    [SerializeField] bool hasRecentlyDug = false;
    [SerializeField] int indexTimer = 0, indexTimerDig = 0;
    [SerializeField] bool hurtsPlayer = true;

    private void Awake() {
        if (pickPoint == null)
            pickPoint = transform;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && hurtsPlayer) {
            EventBus.Publish(new EventFailure());
            Debug.Log("Died to pickaxe");
        }

        breakTileHelper();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player") && hurtsPlayer) {
            EventBus.Publish(new EventFailure());
            Debug.Log("Died to pickaxe");
        }
            
        breakTileHelper();
    }

    private void breakTileHelper() {
        Collider2D[] overCollider = Physics2D.OverlapCircleAll(pickPoint.position, pickaxeHitBoxSize, WhatIsPlatform);

        if (cooldown <= 0) {
            cooldown = mineInterval;
            AudioManager.instance.playSound("3-dig_rocks", mineInterval, pickPoint.position.x, pickPoint.position.y);
            foreach (var hit in overCollider)
                if (hit != null) {
                    hit.transform.GetComponent<MinableTile>().DestroyTileMapAtPoint(pickPoint.position, 1);
                }
        }
    }

    private void Start() {
        cooldown = 0;
        StartCoroutine(ExplosionActivated());
    }

    private void Update() {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }

    private IEnumerator ExplosionActivated() {
        yield return new WaitForSeconds(0.02f);
        DestroyTilesArea();
    }

    private void DestroyTilesArea() {
        if (blastRadius != null) {
            int radius = Mathf.RoundToInt(blastRadius.radius);
            for (var x = -radius; x <= radius; x++) {
                for (var y = -radius; y <= radius; y++) {
                    Vector3 tilePos = new Vector3(transform.position.x + x, transform.position.y + y, 0);
                    float dist = Vector2.Distance(transform.position, tilePos) - 0.001f;

                    if (dist <= radius) {
                        Collider2D overCollider2d = Physics2D.OverlapCircle(tilePos, 0.01f, WhatIsPlatform);
                        if (overCollider2d != null) {
                            overCollider2d.transform.GetComponent<MinableTile>().DestroyTileMapAtPoint(tilePos, 3);
                        }
                    }
                    else if (dist <= (radius + 2)) {
                        Collider2D overCollider2d = Physics2D.OverlapCircle(tilePos, 2f, WhatIsPlatform);
                        if (overCollider2d != null) {
                            overCollider2d.transform.GetComponent<MinableTile>().DestroyTileMapAtPoint(tilePos, 1);
                        }
                    }
                }
            }
        }
    }
}
