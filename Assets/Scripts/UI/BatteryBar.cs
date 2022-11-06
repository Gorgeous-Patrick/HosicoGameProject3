using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{

  Image img;
  [SerializeField] List<SerializablePair<float, Color>> colors;

  void Start()
  {
    img = GetComponent<Image>();
  }

  void Update()
  {
    img.fillAmount = Gameplay.batteryLevel;
    foreach (var kv in colors)
      if (Gameplay.batteryLevel <= kv.first)
      {
        img.color = kv.second;
        break;
      }
  }

}
