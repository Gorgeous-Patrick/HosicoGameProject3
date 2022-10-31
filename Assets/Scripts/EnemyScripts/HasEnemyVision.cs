using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HasEnemyVision : MonoBehaviour
{
    public UnityEvent OnBlinded;
    // public UnityEvent OnDetectPlayer;
    // public UnityEvent OnLosingPlayer;
    // Subscription<EventPlayerInvisible> sub_EventPlayerInvisible;
    // Subscription<EventBlindEnemy> sub_EventBlindEnemy;

    public bool isAlert = false;
    public float currTimer;

    [SerializeField] private GameObject playerObj;

    [SerializeField] private float range = 0f;
    [SerializeField] private float angle = 0f;
    [SerializeField] private Light spotlight;
    [SerializeField] private Light topLight;
    [SerializeField] private LayerMask block;
    [SerializeField] private Color idleColor, detectedColor;

    //private bool isBlinded = false;

    // Start is called before the first frame update
    void Start()
    {
        // sub_EventPlayerInvisible = EventBus.Subscribe<EventPlayerInvisible>(OnPlayerInvisibleDetected);
        range = 8;
        angle = 36;
        isAlert = false;
        if (spotlight == null)
            spotlight = GetComponent<Light>();
        spotlight.color = Color.yellow;
        topLight.color = Color.yellow;
        currTimer = 0f;
        idleColor = Color.yellow;
        detectedColor = Color.red;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlert)
        {
            currTimer++;
            topLight.color = Color.Lerp(topLight.color, detectedColor, Mathf.PingPong(Time.time, 0.21f));
            spotlight.color = Color.Lerp(spotlight.color, detectedColor, Mathf.PingPong(Time.time, 0.21f));
            if (currTimer == 27)
            {
                Debug.Log("End time: " + currTimer);
            }
        }
        else
        {
            if (currTimer > 0) currTimer--;
            topLight.color = Color.Lerp(topLight.color, idleColor, Mathf.PingPong(Time.time, 0.1f));
            spotlight.color = Color.Lerp(spotlight.color, idleColor, Mathf.PingPong(Time.time, 0.1f));
            // if (currTimer <= 0) OnLosingPlayer?.Invoke();
        }

        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            if (Vector3.Distance(playerObj.transform.position, transform.position) < range)
            {
                Vector3 vec = playerObj.transform.position - transform.position;
                bool inRange = Vector3.Angle(transform.forward, vec) < angle ||
                               Vector3.Distance(transform.position, playerObj.transform.position) < 2.7f;
                if (inRange)
                {
                    if(!Physics.Raycast(transform.position, vec, range, block))
                    {
                        isAlert = true;
                        return;
                    }
                }
            }
        }
        isAlert = false;
    }
}
