using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsExplosion : MonoBehaviour
{
    [SerializeField] float DespawnTime = 1.0f;

    private void Start() {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime() {
        yield return new WaitForSeconds(DespawnTime);
        Destroy(this.gameObject);
    }
}
