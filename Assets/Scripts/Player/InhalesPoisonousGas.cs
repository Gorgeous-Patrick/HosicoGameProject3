using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhalesPoisonousGas : MonoBehaviour
{

  public bool inPoison;

  [SerializeField] int _health;
  int health
  {
    get => _health;
    set
    {
      if (value <= 0)
        EventBus.Publish(new EventFailure());
      else if (value >= 100)
        _health = 100;
      else
        _health = value;
    }
  }
  [SerializeField] int damage = 10, restoration = 30;

  float restoreCountdown, hurtCountdown;

  void Start()
  {
    health = 100;
    restoreCountdown = hurtCountdown = 1f;
    inPoison = false;
    EventBus.Subscribe<EventReset>((_) => health = 100);
  }

  // a very dirty solution to work around Unity's limitations
  // in `Update`, information about the player's status is collected
  // and in `LateUpdate`, we know whether the player is being affected by toxic gas
  void LateUpdate()
  {
    if (!inPoison)
    {
      PoisonEffectController.active = false;
      restoreCountdown -= Time.deltaTime;
      if (restoreCountdown <= 0)
      {
        restoreCountdown = 1f;
        health += restoration;
      }
    }
    else
    {
      PoisonEffectController.active = true;
      hurtCountdown -= Time.deltaTime;
      if (hurtCountdown <= 0)
      {
        hurtCountdown = 1f;
        health -= damage;
      }
    }
    // we reset inPoison and wait for toxic gas objects to overwrite this value if
    // their particles collide with the player in the next frame
    inPoison = false;
  }

}
