using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasInventory : MonoBehaviour
{
    Subscription<EventUpdateInventory> sub_EventInventory;
    [SerializeField] int DynamiteCount = 0;

    private void Start() {
        sub_EventInventory = EventBus.Subscribe<EventUpdateInventory>(_AlterInventory);
    }

    private void _AlterInventory(EventUpdateInventory e) {
        if (e.pickup == "Dynamite") {
            DynamiteCount += e.delta;
            EventBus.Publish(new EventUpdatePickupUI { pickup = e.pickup, newAmount = DynamiteCount });
        }
    }

    private void OnDestroy() {
        EventBus.Unsubscribe(sub_EventInventory);
    }
}
