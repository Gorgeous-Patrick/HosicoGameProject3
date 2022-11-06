using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{

  Image img;

  void  Start()
  {
    img = GetComponent<Image>();
  }

  void Update()
  {
    img.fillAmount = Gameplay.batteryLevel;
  }

}
