using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBreakable : MonoBehaviour
{

  [SerializeField] int durability = 3; // takew how many hits to be destroyed
  [SerializeField] bool canHit = true;
  float toughness = 0.3f; // larger: takes more time to mine
  [SerializeField] bool onlyDynamite = false;

  // indicates the time remaining until next durability decrement
  [SerializeField] float countdown;

  void Start()
  {
    countdown = toughness;
  }

  void alterDurability(int delta)
  {
    if (!canHit) return;
    durability += delta;
    // plays animation
    ParticleSystemManager.RequestParticlesAtPositionAndDirection(transform.position, Vector3.up);
    if (GetComponent<SpriteFlasher>() != null) {
            GetComponent<SpriteFlasher>().FlashFunc();
    }
    if (durability <= 0) Destroy(gameObject);
  }

  void OnTriggerStay2D(Collider2D collisionInfo)
  {
    DestroysBreakables breaker = collisionInfo.gameObject.GetComponent<DestroysBreakables>();
    if (breaker == null || (onlyDynamite && !breaker.isExplosion)) return;
    countdown -= Time.deltaTime;
    if (countdown <= 0 || breaker.isExplosion)
    {
      countdown = toughness;
      alterDurability(breaker.durabilityImpact);
    }
  }

}
