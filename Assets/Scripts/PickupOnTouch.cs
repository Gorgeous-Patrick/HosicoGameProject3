using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOnTouch : MonoBehaviour
{
    [SerializeField] string pickupName;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            EventBus.Publish(new EventUpdateInventory { pickup = pickupName, delta = 1 });
            Destroy(this.gameObject);
        }
    }
}
