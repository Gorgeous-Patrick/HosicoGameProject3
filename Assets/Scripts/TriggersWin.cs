using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TriggersWin : MonoBehaviour
{
    Subscription<EventPlayerDied> sub_EventPlayerDied;

    private void Start()
    {
        sub_EventPlayerDied = EventBus.Subscribe<EventPlayerDied>(OnEventPlayerDied);
    }

    private void OnEventPlayerDied(EventPlayerDied obj)
    {
        SceneManager.LoadScene("GameOverScene");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            SceneManager.LoadScene("GameClearScreen");
        }
    }
}
