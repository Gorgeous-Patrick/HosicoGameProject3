using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUITriggerManager : MonoBehaviour
{
    // Eventbus events subscribed to
    Subscription<EventShowHealthUI> UI_event_subscription;
    Animator anim;

    private void Start() {
        UI_event_subscription = EventBus.Subscribe<EventShowHealthUI>(_UIHealthTransition);
        anim = GetComponent<Animator>();
    }

    private void _UIHealthTransition(EventShowHealthUI e) {
        // do nothing if no animator component exists
        if (anim == null) {
            return;
        }

        // fade to black
        if (e.isStart) {
            anim.SetTrigger("start");
        }
        // fade from black to full view
        else if (!e.isStart) {
            anim.SetTrigger("end");

        }
    }
}
