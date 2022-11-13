using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatesPickupUI : MonoBehaviour
{
  Subscription<EventUpdateInventoryUI> sub_EventUpdatePickup;

  private TextMeshProUGUI DynamiteCounterText;

  private void Awake()
  {
    DynamiteCounterText = GetComponent<TextMeshProUGUI>();
    sub_EventUpdatePickup = EventBus.Subscribe<EventUpdateInventoryUI>(_UpdateCount);
  }

  private void _UpdateCount(EventUpdateInventoryUI e)
  {
    if (e.item == "dynamite")
    {
      DynamiteCounterText.text = "x " + e.newAmount;
    }
  }

  private void OnDestroy()
  {
    EventBus.Unsubscribe(sub_EventUpdatePickup);
  }
}
