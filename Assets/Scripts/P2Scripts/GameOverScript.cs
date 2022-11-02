using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetString("currScene", SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            StartCoroutine(WaitToChangeScreen(0f));
        }
    }
    public void OnPlayerDeathChangeScreen()
    {
        StartCoroutine(WaitToChangeScreen(2.5f));
    }

    private IEnumerator WaitToChangeScreen(float sec)
    {
        yield return new WaitForSeconds(sec);
        Debug.Log("well he dead");
        PlayerPrefs.SetString("currScene", SceneManager.GetActiveScene().name);
        Debug.Log("current saved scene name: " + PlayerPrefs.GetString("currScene"));
        SceneManager.LoadScene("GameOverScene");
    }

    public void OnPlayerWinChangeScreen()
    {
        PlayerPrefs.SetString("currScene", SceneManager.GetActiveScene().name);
        Debug.Log("Scenecurr: " + PlayerPrefs.GetString("currScene"));
        if (PlayerPrefs.GetString("currScene") == "FinalBossScene")
            SceneManager.LoadScene("VictoryScene");
        else SceneManager.LoadScene("WinScene");
    }
}
