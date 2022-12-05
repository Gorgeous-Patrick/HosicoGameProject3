using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsesOnQuake : Collapses
{

  protected override Color indicatorColor { get => new Color(255, 255, 0, 0.1f); }

  // if the quake strength at this point is lower than the threshold, no collapse will be triggered
  [SerializeField] int threshold;

  // affects the amount of prefabs generated in a single collapse
  [SerializeField] int strength;

  float countdown;
  int remaining;

  void Start()
  {
    EventBus.Subscribe<EventQuake>(handler_EventQuake);
    countdown = 0;
    remaining = 0;
  }

  void handler_EventQuake(EventQuake e)
  {
    int quakeStrength = e.strengthAt(Utils.flatten(transform.position));
    if (quakeStrength >= threshold)
      remaining += strength * quakeStrength;
      AudioManager.instance.playSound("1-falling_rocks", 1.0f);
  }

  void Update()
  {
    if (countdown > 0)
      countdown -= Time.deltaTime;
    if (remaining > 0 & countdown <= 0)
    {
      remaining--;
      countdown = Random.Range(0, 0.5f);
      // TODO: trigger screen shake
      generate();
    }
  }

}
