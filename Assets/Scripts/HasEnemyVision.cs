using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasEnemyVision : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;

    [SerializeField] private float spd;
    [SerializeField] private float range = 0f;
    [SerializeField] private float angle = 0f;
    [SerializeField] private bool isAlert = false;
    [SerializeField] private Light spotlight;
    [SerializeField] private Light topLight;
    [SerializeField] private LayerMask block;

    [SerializeField] private float detectTimer;
    [SerializeField] private float currTimer; 

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
        detectTimer = 200f;
        currTimer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlert)
        {
            if (currTimer < detectTimer) currTimer++;
            else Debug.Log("Welp Player Died"); //EventBus.Publish(new EventPlayerDied());
        }
        else
        {
            if (currTimer > 0) currTimer--;
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
                        topLight.color = Color.red;
                        spotlight.color = Color.red;
                        return;
                    }
                }
            }
        }
        topLight.color = Color.yellow;
        spotlight.color = Color.yellow;
    }
}
