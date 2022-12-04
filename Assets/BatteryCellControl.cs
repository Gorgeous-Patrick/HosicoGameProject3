using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCellControl : MonoBehaviour
{
    public int batteryCellId = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<EventBlinkBatteryBar>(handler_blinkbattery);
    }

    void OnDestroy()
    {
    }

    void handler_blinkbattery(EventBlinkBatteryBar e) 
    {
        if (e.prevBatteryLevel == batteryCellId + 1)
        {
            StartCoroutine(BlinkBattery());
            Debug.Log("Blinking Battery");
            Debug.Log("Battery Cell Id: " + batteryCellId);
            Debug.Log("Current Battery Level: " + Gameplay.batteryLevel);
        }
    }

    IEnumerator BlinkBattery()
    {
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        enabled = false;
    }
}
