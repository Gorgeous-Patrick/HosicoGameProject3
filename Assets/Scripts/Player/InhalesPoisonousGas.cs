using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhalesPoisonousGas : MonoBehaviour
{

  public bool inPoison;

  [SerializeField] int _health;
  public int health
  {
    get => _health;
    set
    {
      if (value >= 100)
        _health = 100;
      else
        _health = value;
    }
  }
  [SerializeField] int damage, restoration;

  [SerializeField] float restoreCountdown, hurtCountdown;

  void Start()
  {
    inPoison = false;
    reset();
  }

  public void reset()
  {
    health = 100;
    restoreCountdown = 1f;
    hurtCountdown = 0.1f;
  }

  // a very dirty solution to work around Unity's limitations
  // in `Update`, information about the player's status is collected
  // and in `LateUpdate`, we know whether the player is being affected by toxic gas
  void LateUpdate()
  {
    PoisonEffectController.active = (health != 100);
    if (!inPoison)
    {
      restoreCountdown -= Time.deltaTime;
      if (restoreCountdown <= 0)
      {
        restoreCountdown = 1f;
        health += restoration;
      }
    }
    else
    {
      hurtCountdown -= Time.deltaTime;
      if (hurtCountdown <= 0)
      {
        hurtCountdown = 0.1f;
        health -= damage;
      }
    }
    // we reset inPoison and wait for toxic gas objects to overwrite this value if
    // their particles collide with the player in the next frame
    inPoison = false;
  }

    private void Update()
    {
        if (health == 0)
        {
            EventBus.Publish(new EventFailure { noZoomIn = true });
            reset();
        }
    }

}
