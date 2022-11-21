using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadsSceneOnClick : MonoBehaviour
{
    [SerializeField] bool isClicked = false;

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
        Debug.Log("Analytics Start Error");
        }
    }
    [SerializeField] string SceneToLoad = "Level 1";
    [SerializeField] GameObject UserConsent;

    public void LoadScene() {
        if (isClicked) { return;  };
        StartCoroutine(ClickSequence());
    }

    IEnumerator ClickSequence() {
        isClicked = true;
        EventBus.Publish<EventStartTransition>(new EventStartTransition{ isStart = true });
        yield return new WaitForSeconds(1.75f);

        if (SceneManager.GetActiveScene().name != "Title")
            SceneToLoad = PlayerPrefs.GetString("currScene");
        SceneManager.LoadScene(SceneToLoad);
        if (UserConsent.GetComponent<Toggle>().isOn) {
            startAnalytics();
        }
    }
}
