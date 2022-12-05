using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsExplosion : MonoBehaviour
{
  [SerializeField] float DespawnTime = 0.1f;
  [SerializeField] int quakeStrength = 5;

  void Start()
  {
    EventBus.Publish(new EventQuake{source = transform.position, initialStrength = quakeStrength});
    StartCoroutine(DestroyAfterTime());
  }

  IEnumerator DestroyAfterTime()
  {
    yield return new WaitForSeconds(DespawnTime);
    Destroy(this.gameObject);
  }
}
