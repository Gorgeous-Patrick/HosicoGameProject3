using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowsPlayerLookDirection : MonoBehaviour
{
  void Start()
  {
    EventBus.Subscribe<Event_PlayerLookDirectionChanged>(updateRotation);
  }

  void updateRotation(Event_PlayerLookDirectionChanged e)
  {
    transform.up = e.dirvec;
  }
}
