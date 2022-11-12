using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdatesPickupUI : MonoBehaviour
{
    Subscription<EventUpdatePickupUI> sub_EventUpdatePickup;

    private TextMeshProUGUI DynamiteCounterText;

    private void Awake() {
        DynamiteCounterText = GetComponent<TextMeshProUGUI>();
        sub_EventUpdatePickup = EventBus.Subscribe<EventUpdatePickupUI>(_UpdateCount);
    }

    private void _UpdateCount(EventUpdatePickupUI e) {
        if (e.pickup == "Dynamite") {
            DynamiteCounterText.text = "x " + e.newAmount;
        }
    }

    private void OnDestroy() {
        EventBus.Unsubscribe(sub_EventUpdatePickup);
    }
}
