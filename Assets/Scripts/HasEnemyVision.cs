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
    }

    // Update is called once per frame
    void Update()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        if(playerObj != null)
        {
            Vector3 vec = playerObj.transform.position - transform.position;
            if(Vector3.Distance(playerObj.transform.position, transform.position) < range)
            {
                if(Vector3.Angle(transform.forward, vec) < angle)
                {
                    isAlert = true;
                    topLight.color = Color.red;
                    spotlight.color = Color.red;
                    return;
                }
            }
        }
        topLight.color = Color.yellow;
        spotlight.color = Color.yellow;
    }
}
