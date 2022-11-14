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
        if (targetSceneName != null)
            GameObject.Find("Player").transform.position = destination.transform.position;
    else SceneManager.LoadScene(targetSceneName);
  }
}
