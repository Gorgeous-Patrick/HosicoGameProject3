using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggersWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") {
            SceneManager.LoadScene("GameClearScreen");
        }
    }
}
