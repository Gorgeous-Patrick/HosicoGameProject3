using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    AudioSource chargeSFX;

    private void Start() {
        chargeSFX = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (chargeSFX != null) {
            chargeSFX.Play();
        }
        EventBus.Publish(new EventBatteryStatusChange { charging = true });
    }

    void OnTriggerExit2D(Collider2D collision) {
        EventBus.Publish(new EventBatteryStatusChange { charging = false });
    }

}
