using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadsSceneOnClick : MonoBehaviour
{
    [SerializeField] string SceneToLoad = "Level 1";

    public void LoadScene() {
        Debug.Log("Clicked.");
        SceneManager.LoadScene(SceneToLoad);
    }

}
