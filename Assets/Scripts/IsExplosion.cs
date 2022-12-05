using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsExplosion : MonoBehaviour
{
  [SerializeField] float DespawnTime = 1.0f;
  [SerializeField] int quakeStrength = 5;

  void Start()
  {
    EventBus.Publish(new EventQuake{source = transform.position, initialStrength = quakeStrength});
        EventBus.Publish(new OnCollapseScreenShakeEvent { intensity = 3, time = 2 });
    StartCoroutine(DestroyAfterTime());
  }

  IEnumerator DestroyAfterTime()
  {
    yield return new WaitForSeconds(DespawnTime);
    Destroy(this.gameObject);
  }
}
