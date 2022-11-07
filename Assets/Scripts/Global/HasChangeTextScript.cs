using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HasChangeTextScript : MonoBehaviour
{
    Subscription<EventLoseHealth> sub_EventLoseHealth;

    private TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Awake()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        sub_EventLoseHealth = EventBus.Subscribe<EventLoseHealth>(OnEventLoseHealthDo);
    }

    private void OnEventLoseHealthDo(EventLoseHealth obj)
    {
        healthText.text = "x " + obj.health;
    }
}
