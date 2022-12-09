using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivateElevator : Interactive
{
    public UnityEvent disablePromptEvent;

    [SerializeField] string targetSceneName;
    [SerializeField] GameObject Background;

    bool activated = false;

    protected override void Interact() {
        Gameplay.playerInput.Disable();
        disablePromptEvent?.Invoke();
        activated = true;
    }

    void Update() {
        if (activated) {
            transform.Translate(Vector2.up * 3f * Time.deltaTime);
            if (Background != null) {
                Background.GetComponent<Image>().color = Color.white;
            }
            
            StartCoroutine(TransitionCoroutine());
        }
    }

    IEnumerator TransitionCoroutine() {
        // buy more time for elevator to go beyond screen edge
        yield return new WaitForSeconds(1.5f);

        EventBus.Publish<EventStartTransition>(new EventStartTransition { isExtendedStart = true });
        yield return new WaitForSeconds(4.0f);

        // load target scene
        SceneManager.LoadScene(targetSceneName);
    }
}
