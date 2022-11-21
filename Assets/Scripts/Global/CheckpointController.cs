using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
  static CheckpointController _instance;
  public static CheckpointController instance
  {
    get
    {
      return _instance;
    }
  }

  void Awake()
  {
    if (_instance != null && _instance != this)
      Destroy(this.gameObject);
    else
      _instance = this;
  }

  [SerializeField] Sprite DeathSprite;
  [SerializeField] int maxHealth = 3;

  bool invincible;

  int playerHealth;
  Vector2 _checkpoint;
  int checkpointIdx;

  static public Vector2 checkpoint
  {
    get => instance._checkpoint;
  }

  void Start()
  {
    EventBus.Subscribe<EventFailure>(handler_EventFailure);
    EventBus.Subscribe<EventToggleInvincibility>((e) => invincible = e.invincible);
    playerHealth = maxHealth;
    _checkpoint = Gameplay.player.transform.position;
    checkpointIdx = 0;
    invincible = false;
  }

    static public void updateCheckpoint(int index, Vector2 checkpoint)
  {

    if (index > instance.checkpointIdx)
      instance._checkpoint = checkpoint;
  }

  void handler_EventFailure(EventFailure _)
  {
    if (invincible) return;
    invincible = true;
    playerHealth--;
    EventBus.Publish(new EventUpdateHealth { newHealth = playerHealth });
    // !!! We expect the EventReset handler to disable invincible mode
    EventBus.Publish(new EventReset {resetEntireLevel = playerHealth <= 0});
  }

}
