using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsExplosion : MonoBehaviour
{
    public UnityEvent OnExplodeShake;
  [SerializeField] float DespawnTime = 0.1f;
  [SerializeField] int quakeStrength = 5;

  void Start()
  {
    EventBus.Publish(new EventQuake{source = transform.position, initialStrength = quakeStrength});
        OnExplodeShake?.Invoke();
    StartCoroutine(DestroyAfterTime());
  }

  IEnumerator DestroyAfterTime()
  {
    yield return new WaitForSeconds(DespawnTime);
    Destroy(this.gameObject);
  }
}
