using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsBreakable : MonoBehaviour
{
    public UnityEvent OnHitFlash;

    [SerializeField] float durability = 1.0f;
    [SerializeField] float despawnTimer = 1.5f;
    [SerializeField] bool canHit = true;

    public void AlterDurability(float delta) {
        if (!canHit) {
            return;
        }

        durability += delta;
        if (OnHitFlash != null) {
            OnHitFlash?.Invoke();
        }
        ParticleSystemManager.RequestParticlesAtPositionAndDirection(transform.position, Vector3.up);

        if (durability <= 0.0f)
        {
            StartCoroutine(Destroy());
        }
    }

    // called to destroy gameObject
    // Could add call for more detailed destroyed animations later down the line
    IEnumerator Destroy() {
        canHit = false;

        // if sprite exists, change to black
        if (gameObject.GetComponent<SpriteRenderer>() != null) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        // wait for time specified by despawnTimer, then despawn
        yield return new WaitForSeconds(despawnTimer);
        Destroy(this.gameObject);
    }
}
