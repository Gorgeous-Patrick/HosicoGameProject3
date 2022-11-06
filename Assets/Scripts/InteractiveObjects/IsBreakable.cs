using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsBreakable : MonoBehaviour
{
    public UnityEvent OnHitFlash;

    [SerializeField] float durability = 1.0f;
    [SerializeField] float despawnTimer = 0.5f;

    public void AlterDurability(float delta) {
        durability += delta;
        if (durability <= 0.0f)
        {
            StartCoroutine(Destroy());
        }
        else OnHitFlash?.Invoke();
    }

    // called to destroy gameObject
    // Could add call for more detailed destroyed animations later down the line
    IEnumerator Destroy() {
        // if sprite exists, change to black
        if (gameObject.GetComponent<SpriteRenderer>() != null) {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        // wait for time specified by despawnTimer, then despawn
        yield return new WaitForSeconds(despawnTimer);
        Destroy(this.gameObject);
    }
}
