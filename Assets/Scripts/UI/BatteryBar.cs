using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{

  Image img;
  [SerializeField] List<SerializablePair<float, Color>> colors;
  private bool flag = false;

  void Start()
  {
    img = GetComponent<Image>();
  }

  void Update()
  {
    img.fillAmount = Gameplay.batteryLevel / 5f;
    foreach (var kv in colors)
      if (Gameplay.batteryLevel <= kv.first)
      {
        img.color = kv.second;
        if (flag == false && kv.first == 0.3f) {
          AudioManager.instance.playSound("8-low_battery", 1.0f);
          Debug.Log("switch");
          flag = true;
      }
        break;
      }
      
  }

}
