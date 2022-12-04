using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    AudioSource chargeSFX;
    [SerializeField] int generatorIndex = 0;

    private void Start() {
        chargeSFX = GetComponent<AudioSource>();
    }

    void OnChargingStart()
    {
        if (chargeSFX != null) {
            chargeSFX.Play();
        }
        EventBus.Publish(new EventBatteryStatusChange { charging = true });
    }

    void OnChargingEnd() 
    {
        EventBus.Publish(new EventBatteryStatusChange { charging = false });
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnChargingStart();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnChargingEnd();
        }
    }

}
