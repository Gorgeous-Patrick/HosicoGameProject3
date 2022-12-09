using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ActivateElevator : MonoBehaviour
{
    public UnityEvent DisablePromptEvent;

    [SerializeField] string targetSceneName;

    bool activated = false;
    bool isPlayerInElevator = false;
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Update()
    {
        if (isPlayerInElevator)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Gameplay.playerInput.Disable();
                DisablePromptEvent?.Invoke();
                activated = true;
                StartCoroutine(TransitionCoroutine());
            }
        }
        if(activated)
            transform.Translate(Vector2.up * 3f * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.transform.CompareTag("Player"))
        {
            Debug.Log("PlayerOn");
            isPlayerInElevator = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.transform.CompareTag("Player"))
        {
            isPlayerInElevator = false;
        }
    }

    IEnumerator TransitionCoroutine()
    {
        EventBus.Publish<EventStartTransition>(new EventStartTransition { isStart = true });
        yield return new WaitForSeconds(1.75f);

        // load target scene
        SceneManager.LoadScene(targetSceneName);
    }
}
