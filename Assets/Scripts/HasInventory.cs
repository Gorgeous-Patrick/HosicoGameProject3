using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasInventory : MonoBehaviour
{
  [SerializeField] int DynamiteCount = 0;
  [SerializeField] GameObject dynamitePrefab;

  void Start()
  {
    EventBus.Subscribe<EventUpdateInventory>(_AlterInventory);
  }

  void _AlterInventory(EventUpdateInventory e)
  {
    if (e.pickup == "Dynamite")
    {
      DynamiteCount += e.delta;
      EventBus.Publish(new EventUpdatePickupUI { pickup = e.pickup, newAmount = DynamiteCount });
    }
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.X))
    {
      if (dynamitePrefab != null && DynamiteCount > 0)
      {
        DynamiteCount -= 1;
        EventBus.Publish(new EventUpdatePickupUI { pickup = "Dynamite", newAmount = DynamiteCount });
        Instantiate(dynamitePrefab, transform.position, Quaternion.identity);
      }
    }
  }
}
