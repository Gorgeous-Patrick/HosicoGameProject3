using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file is only for testing purposes
// Any keybinding here should be later merged into the input system

public class CheatKeys : MonoBehaviour
{

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      EventBus.Publish(new EventQuake {initialStrength = 5, source = transform.position});
    }
  }

}
