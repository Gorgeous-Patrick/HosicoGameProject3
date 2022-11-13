using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplaysHealth : MonoBehaviour
{

  private TextMeshProUGUI healthText;

  void Awake()
  {
    healthText = GetComponent<TextMeshProUGUI>();
    EventBus.Subscribe<EventLoseHealth>(OnEventLoseHealthDo);
  }

  private void OnEventLoseHealthDo(EventLoseHealth obj)
  {
    healthText.text = "x " + obj.health;
  }
}
