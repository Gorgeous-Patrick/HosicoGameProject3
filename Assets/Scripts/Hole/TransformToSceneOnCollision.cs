using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformToSceneOnCollision : MonoBehaviour
{
    [SerializeField] string targetSceneName;
    [SerializeField] GameObject keyPrompt;
    bool isTriggered = false;

    void enablePromptUserToPressKey()
    {
        keyPrompt.SetActive(true);
    }

    void disablePromptUserToPressKey()
    {
        keyPrompt.SetActive(false);
    }

    void Start()
    {
        disablePromptUserToPressKey();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player trigger detected");
            isTriggered = true;
            enablePromptUserToPressKey();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTriggered = false;
            disablePromptUserToPressKey();
        }
    }

    void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }

}
