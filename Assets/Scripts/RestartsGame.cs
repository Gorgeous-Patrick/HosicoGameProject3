using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartsGame : MonoBehaviour
{
    [SerializeField] string SceneToLoad = "Level 1";

    public void RestartGame() {
        SceneManager.LoadScene(SceneToLoad);
    }
}
