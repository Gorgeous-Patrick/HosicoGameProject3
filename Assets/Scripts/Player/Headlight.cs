using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // for Light2D

public class Headlight : MonoBehaviour
{

  Light2D light;
  float maxIntensity;
  [SerializeField] float minimumIntensityRatio = 0.3f;
  float minIntensity
  {
    get => maxIntensity * minimumIntensityRatio;
  }

  void Start()
  {
    light = GetComponent<Light2D>();
    maxIntensity = light.intensity;
    EventBus.Subscribe<EventHeadlightStatusChange>(handler_EventHeadlightStatusChange);
  }

  void Update()
  {
    light.intensity = minIntensity + Gameplay.batteryLevel * (maxIntensity - minIntensity);
  }

  void handler_EventHeadlightStatusChange(EventHeadlightStatusChange e)
  {
    light.enabled = e.enabled;
  }

}
