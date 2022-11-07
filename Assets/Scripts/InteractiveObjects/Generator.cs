using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactive
{

  protected override void Interact()
  {
    EventBus.Publish(new EventBatteryStatusChange { charging = true });
    EventBus.Publish(new EventChangeCheckpoint {checkpoint = transform});
        PlayerPrefs.SetFloat("x-pos", transform.position.x);
        PlayerPrefs.SetFloat("y-pos", transform.position.y);
        PlayerPrefs.SetFloat("z-pos", transform.position.z);
        Debug.Log("Player random prefs" + PlayerPrefs.GetFloat("z-pos"));
  }

  protected override void Interrupt()
  {
    EventBus.Publish(new EventBatteryStatusChange { charging = false });
  }
}
