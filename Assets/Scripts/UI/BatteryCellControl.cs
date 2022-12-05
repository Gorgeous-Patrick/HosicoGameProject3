using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryCellControl : MonoBehaviour
{
  public int batteryCellId = 0;
  bool blinking = false;

  void Start()
  {
    EventBus.Subscribe<EventBlinkBatteryBar>(handler_blinkbattery);
  }

  void Update()
  {
    Color color;
    if (Gameplay.batterys > batteryCellId)
    {
      color = Color.green;
    }
    else
    {
      color = Color.white;
    }

    if (!blinking)
    {
      GetComponent<Image>().color = color;
    }

  }

  void handler_blinkbattery(EventBlinkBatteryBar e)
  {
    if (e.prevBatteryLevel == batteryCellId + 1)
    {
      StartCoroutine(BlinkBattery());
      Debug.Log("Blinking Battery");
      Debug.Log("Battery Cell Id: " + batteryCellId);
      Debug.Log("Current Battery Level: " + Gameplay.batterys);
    }
  }

  IEnumerator BlinkBattery()
  {
    blinking = true;
    for (int i = 0; i < 3; i++)
    {
      GetComponent<Image>().color = Color.red;
      yield return new WaitForSeconds(0.1f);
      GetComponent<Image>().color = Color.white;
      yield return new WaitForSeconds(0.1f);
    }
    blinking = false;
  }
}
