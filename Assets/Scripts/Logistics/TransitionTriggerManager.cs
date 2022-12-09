using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTriggerManager : MonoBehaviour
{
    // Eventbus events subscribed to
    Subscription<EventStartTransition> transition_event_subscription;
    Animator anim;

    private void Start() {
        transition_event_subscription = EventBus.Subscribe<EventStartTransition>(_TriggerTransition);
        anim = GetComponent<Animator>();
    }

    private void _TriggerTransition(EventStartTransition e) {
        // do nothing if no animator component exists
        if (anim == null) {
            return;
        }
        // fade to black, but extended
        if (e.isExtendedStart) {
            anim.SetTrigger("start_extend");
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
