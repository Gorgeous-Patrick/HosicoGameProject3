using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
  AudioSource chargeSFX;
  [SerializeField] int generatorIndex = 0;

  private void Start()
  {
    chargeSFX = GetComponent<AudioSource>();
  }

  void OnTriggerEnter2D(Collider2D collisionInfo)
  {
    if (collisionInfo.gameObject == Gameplay.player)
    {
      if (chargeSFX != null)
        chargeSFX.Play();
      EventBus.Publish(new EventBatteryStatusChange { charging = true });
    }
  }

  void OnTriggerExit2D(Collider2D collisionInfo)
  {
    if (collisionInfo.gameObject == Gameplay.player)
      EventBus.Publish(new EventBatteryStatusChange { charging = false });
  }

}
