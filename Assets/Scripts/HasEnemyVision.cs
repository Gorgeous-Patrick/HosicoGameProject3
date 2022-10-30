using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasEnemyVision : MonoBehaviour
{
    public bool isAlert = false;
    public float currTimer;

    [SerializeField] private GameObject playerObj;

    [SerializeField] private float spd;
    [SerializeField] private float range = 0f;
    [SerializeField] private float angle = 0f;
    [SerializeField] private Light spotlight;
    [SerializeField] private Light topLight;
    [SerializeField] private LayerMask block;

    // [SerializeField] private float detectTimer;
    [SerializeField] private Color idleColor, detectedColor;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        range = 9;
        angle = 23;
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
            Debug.Log("End time: " + currTimer);
        }
        else
        {
            if (currTimer > 0) currTimer--;
            topLight.color = Color.Lerp(topLight.color, idleColor, Mathf.PingPong(Time.time, 0.1f));
            spotlight.color = Color.Lerp(spotlight.color, idleColor, Mathf.PingPong(Time.time, 0.1f));
        }

        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Vector3 vec = playerObj.transform.position - transform.position;
            if (Vector3.Distance(playerObj.transform.position, transform.position) < range)
            {
                bool inRange = Vector3.Angle(transform.forward, vec) < angle ||
                               Vector3.Distance(transform.position, playerObj.transform.position) < 2;
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
