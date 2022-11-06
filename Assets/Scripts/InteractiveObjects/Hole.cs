using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Hole : Interactive
{
    [SerializeField] string targetSceneName;

    protected override void Interact()
    {
      SceneManager.LoadScene(targetSceneName);
    }
}
