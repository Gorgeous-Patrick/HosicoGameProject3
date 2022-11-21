using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOnTouch : MonoBehaviour
{
  [SerializeField] string pickupName;
  [SerializeField] int amount = 1;

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      EventBus.Publish(new EventUpdateInventory { pickup = pickupName, delta = amount });
      AudioManager.instance.playSound("7-pick_up", 1.0f);
      
      Destroy(this.gameObject);
    }
  }
}
