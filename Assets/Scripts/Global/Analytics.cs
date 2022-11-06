using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

struct DataFrame 
{
    float x, y;
    string sceneName;
}


public class Analytics : MonoBehaviour
{    
    async void Start()
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
}
