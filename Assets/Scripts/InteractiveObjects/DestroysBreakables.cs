using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroysBreakables : MonoBehaviour
{
    [SerializeField] float durabilityDamage = -1.0f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<IsBreakable>() != null) {
            Debug.Log("Hit breakable object.");
            collision.gameObject.GetComponent<IsBreakable>().AlterDurability(durabilityDamage);
        }
        if (collision.gameObject.CompareTag("BreakableGround"))
        {
            Debug.Log("hit breakable ground");
            collision.gameObject.GetComponent<HasCanBeDugScript>().DestroyTileMapAtPoint(transform.position);
        }
    }
}
