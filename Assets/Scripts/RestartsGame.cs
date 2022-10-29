using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartsGame : MonoBehaviour
{
    public void RestartGame() {
        SceneManager.LoadScene("Level 1");
    }
}
