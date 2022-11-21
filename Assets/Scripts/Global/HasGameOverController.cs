using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HasGameOverController : MonoBehaviour
{

  [SerializeField] Sprite DeathSprite;
  [SerializeField] String GameOverScene;
  [SerializeField] Transform firstCheckpoint;
  [SerializeField] private int maxHealth = 3;
  private int playerHealth;
  private Vector2 respawnPos;

  void Awake()
  {
    EventBus.Subscribe<EventFailure>(OnEventFailureDo);
    EventBus.Subscribe<EventChangeCheckpoint>(OnCheckpointReachedDo);
    playerHealth = maxHealth;
    respawnPos = firstCheckpoint.position;
  }

  private void Update()
  {
    if (playerHealth <= 0)
    {
      playerHealth = maxHealth;
      // loads Game Over scene
      PlayerPrefs.SetString("currScene", SceneManager.GetActiveScene().name);
      SceneManager.LoadScene(GameOverScene);
    }
  }

  private void OnCheckpointReachedDo(EventChangeCheckpoint obj)
  {
    respawnPos = obj.checkpoint.position;
  }

  private void OnEventFailureDo(EventFailure obj)
  {
    playerHealth--;

    if (playerHealth > 0)
    {
      EventBus.Publish(new EventLoseHealth { health = playerHealth });
      EventBus.Publish(new EventResetPlayer { pos = respawnPos });
    }
    else
    {
      AudioManager.instance.playSound("4-player_death", 1.0f);
      // AudioManager.instance.playSound("3-dig_rocks", 1.0f);
      Debug.Log("Gameover");
    }
  }
}
