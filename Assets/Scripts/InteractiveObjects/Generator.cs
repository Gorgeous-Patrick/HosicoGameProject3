using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        EventBus.Publish(new EventBatteryStatusChange { charging = true });
        EventBus.Publish(new EventChangeCheckpoint { checkpoint = transform });
    }

    private void OnTriggerExit2D(Collider2D collision) {
        EventBus.Publish(new EventBatteryStatusChange { charging = false });
    }

}
