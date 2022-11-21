using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplaysHealth : MonoBehaviour
{

  private TextMeshProUGUI healthText;

  void Start()
  {
    healthText = GetComponent<TextMeshProUGUI>();
    EventBus.Subscribe<EventUpdateHealth>((e) => healthText.text = $"x {e.newHealth}");
  }

}
