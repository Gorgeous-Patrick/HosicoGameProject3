using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplaysHealth : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    Subscription<EventShowHealthUI> UI_event_subscription;
    Animator anim;

    void Start() {
        healthText = GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();

        EventBus.Subscribe<EventUpdateHealth>((e) => healthText.text = $"x {e.newHealth}");
        UI_event_subscription = EventBus.Subscribe<EventShowHealthUI>(_changeRed);
    }

    private void _changeRed(EventShowHealthUI e) {
        if (e.isRed) {
            if (healthText != null) {
                if (anim != null)
                    anim.SetTrigger("red");
            }
        }
    }

}
