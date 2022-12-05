using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // for Light2D

public class Headlight : MonoBehaviour
{

  Light2D light2d;
  float maxIntensity;
  [SerializeField] float minimumIntensityRatio = 0.3f;
  float minIntensity
  {
    get => maxIntensity * minimumIntensityRatio;
  }

  void Start()
  {
    light2d = GetComponent<Light2D>();
    maxIntensity = light2d.intensity;
    EventBus.Subscribe<EventHeadlightStatusChange>(handler_EventHeadlightStatusChange);
  }

  void Update()
  {
    light2d.intensity = minIntensity + Gameplay.batterys * (maxIntensity - minIntensity);
  }

  void handler_EventHeadlightStatusChange(EventHeadlightStatusChange e)
  {
    light2d.enabled = e.enabled;
  }

}
