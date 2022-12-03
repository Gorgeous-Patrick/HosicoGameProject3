using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.UI;

public class LoadsSceneOnClick : MonoBehaviour
{
    

    async void startAnalytics()
    {
        Debug.Log("Analytics Start");
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            Debug.Log(e);
        Debug.Log("Analytics Start Error");
        }
    }

    [SerializeField] string SceneToLoad = "Level 1";
    [SerializeField] GameObject UserConsent;
    [SerializeField] bool isClicked = false;
    [SerializeField] bool isButton = false;

    AudioSource clickSFX;
    private void Start() {
        clickSFX = GetComponent<AudioSource>();
    }

    public void LoadScene() {
        if (isClicked) { return;  };
        StartCoroutine(ClickSequence());
    }

    IEnumerator ClickSequence() {
        // prevent spamming
        isClicked = true;

        // start audio clip 
        if (clickSFX != null) {
            clickSFX.Play();
        }

        // call for transition
        EventBus.Publish<EventStartTransition>(new EventStartTransition{ isStart = true });
        yield return new WaitForSeconds(1.75f);

        // additional time to allow audio to finish playing - currently for title screen
        yield return new WaitForSeconds(2.25f);

        if (SceneManager.GetActiveScene().name != "Title" && !isButton)
            SceneToLoad = PlayerPrefs.GetString("currScene");
        SceneManager.LoadScene(SceneToLoad);
        if (UserConsent.GetComponent<Toggle>().isOn) {
            startAnalytics();
        }
    }
}
