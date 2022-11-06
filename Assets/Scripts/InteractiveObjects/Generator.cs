using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactive
{

  protected override void Interact()
  {
    EventBus.Publish(new EventBatteryStatusChange { charging = true });
  }

  protected override void Interrupt()
  {
    EventBus.Publish(new EventBatteryStatusChange { charging = false });
  }

}
