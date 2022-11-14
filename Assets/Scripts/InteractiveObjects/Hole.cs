using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Hole : Interactive
{
  [SerializeField] Transform destination = null;
  [SerializeField] string targetSceneName;

  protected override void Interact()
  {
        if (destination != null)
        {
            if (GameObject.Find("Player") != null)
                GameObject.Find("Player").transform.position = destination.transform.position;
            else GameObject.Find("PlayerTutorial").transform.position = destination.transform.position;

        }

        else if (targetSceneName != "None") SceneManager.LoadScene(targetSceneName);
        else return;
  }
}
