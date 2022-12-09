using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Hole : Interactive
{
  [SerializeField] Transform destination = null;
  [SerializeField] string targetSceneName;
  [SerializeField] bool midTransition = false;

  protected override void Interact()
  {
    if (midTransition)
    {
      return;
    }

    StartCoroutine(TransitionSequence());
  }

  IEnumerator TransitionSequence()
  {
    midTransition = true;

    // if there is a destination referenced, call and allow for transition, then teleport player
    if (destination != null)
    {
      // call and wait for start transition
      EventBus.Publish<EventStartTransition>(new EventStartTransition { isStart = true });
      yield return new WaitForSeconds(1.75f);

      if (GameObject.Find("Player") != null)
        GameObject.Find("Player").transform.position = destination.transform.position;
      else GameObject.Find("PlayerTutorial").transform.position = destination.transform.position;

      // call and wait for end transition
      EventBus.Publish<EventStartTransition>(new EventStartTransition { isStart = false });
      yield return new WaitForSeconds(1.75f);

      midTransition = false;
    }
    // else if there is a target scene to load instead
    else if (targetSceneName != "None")
    {
      // call and wait for start transition
      EventBus.Publish<EventStartTransition>(new EventStartTransition { isStart = true });
      yield return new WaitForSeconds(1.75f);

      // load target scene
      SceneManager.LoadScene(targetSceneName);
    }
    else
    {
      midTransition = false;
      yield return null;
    }

  }
}
