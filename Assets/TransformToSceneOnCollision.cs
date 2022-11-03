using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransformToSceneOnCollision : MonoBehaviour
{
    public string targetSceneName;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision with player detected");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
