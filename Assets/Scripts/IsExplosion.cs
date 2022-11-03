using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsExplosion : MonoBehaviour
{
    [SerializeField] float respawnTime = 1.0f;

    private void Start() {
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime() {
        yield return new WaitForSeconds(respawnTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Boulder") {
            Debug.Log("Collided");
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
