using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOnTouch : MonoBehaviour
{
    [SerializeField] string pickupName;
    [SerializeField] int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            EventBus.Publish(new EventUpdateInventory { pickup = pickupName, delta = amount });
            Destroy(this.gameObject);
        }
    }
}
