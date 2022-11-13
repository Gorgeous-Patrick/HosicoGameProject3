using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsBreakable : MonoBehaviour
{

  [SerializeField] int durability = 1; // takew how many hits to be destroyed
  [SerializeField] bool canHit = true;
  [SerializeField] float toughness = 1f; // larger: takes more time to mine

  // indicates the time remaining until next durability decrement
  float countdown;

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
    if (durability <= 0) Destroy(gameObject);
  }

  void OnTriggerStay2D(Collider2D collisionInfo)
  {
    DestroysBreakables breaker = collisionInfo.gameObject.GetComponent<DestroysBreakables>();
    if (breaker == null) return;
    countdown -= Time.deltaTime;
    if (countdown <= 0)
    {
      countdown = toughness;
      alterDurability(breaker.durabilityImpact);
    }
  }

}
