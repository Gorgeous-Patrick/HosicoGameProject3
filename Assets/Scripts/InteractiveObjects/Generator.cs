using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

  void OnTriggerEnter2D(Collider2D collision)
  {
    EventBus.Publish(new EventBatteryStatusChange { charging = true });
  }

  void OnTriggerExit2D(Collider2D collision)
  {
    EventBus.Publish(new EventBatteryStatusChange { charging = false });
  }

}
