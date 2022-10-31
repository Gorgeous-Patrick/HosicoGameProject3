using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ShowPromptScript : MonoBehaviour
{
    public UnityEvent OnShowText;
    public UnityEvent OnHideText;

    private bool isAlreadyShown = false;
    Subscription<EventpromptShoot> sub_EventpromptShoot;
    // Start is called before the first frame update
    void Start()
    {
        sub_EventpromptShoot = EventBus.Subscribe<EventpromptShoot>(OnCollectedPickup);
    }

    private void OnCollectedPickup(EventpromptShoot obj)
    {
        OnShowText?.Invoke();
        if(!isAlreadyShown) StartCoroutine(WaitToHide());
        isAlreadyShown = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WaitToHide()
    {
        yield return new WaitForSeconds(3.5f);
        OnHideText?.Invoke();
        isAlreadyShown = false;
    }
}
