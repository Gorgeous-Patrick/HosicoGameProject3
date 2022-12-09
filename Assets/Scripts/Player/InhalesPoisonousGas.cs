using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhalesPoisonousGas : MonoBehaviour
{

  public bool inPoison;

  public int health;
  [SerializeField] int damage, restoration;

  float restoreCountdown, hurtCountdown;

  void Start()
  {
    inPoison = false;
    reset();
  }

  public void reset()
  {
    health = 100;
    restoreCountdown = 0.1f;
    hurtCountdown = 0.1f;
  }

  // a very dirty solution to work around Unity's limitations
  // in `Update`, information about the player's status is collected
  // and in `LateUpdate`, we know whether the player is being affected by toxic gas
  void LateUpdate()
  {
    PoisonEffectController.active = (health < 100);
    if (!inPoison)
    {
      restoreCountdown -= Time.deltaTime;
      if (restoreCountdown <= 0)
      {
        restoreCountdown = 0.1f;
        health += restoration;
        if (health > 100) health = 100;
      }
    }
    else
    {
      hurtCountdown -= Time.deltaTime;
      if (hurtCountdown <= 0)
      {
        hurtCountdown = 0.2f;
        health -= damage;
      }
    }
    if (health <= 39) EventBus.Publish(new EventFailure {noZoomIn = true});
    // we reset inPoison and wait for toxic gas objects to overwrite this value if
    // their particles collide with the player in the next frame
    inPoison = false;
  }

}
