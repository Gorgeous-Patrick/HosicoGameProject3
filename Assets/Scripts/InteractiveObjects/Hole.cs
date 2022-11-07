using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Hole : Interactive
{
  [SerializeField] Transform destination;
  // [SerializeField] string targetSceneName;

  protected override void Interact()
  {
    GameObject.Find("Player").transform.position = destination.transform.position;
    // SceneManager.LoadScene(targetSceneName);
  }
}
