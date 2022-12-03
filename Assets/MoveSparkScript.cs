using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSparkScript : MonoBehaviour
{
    public ParticleSystem system;

    [SerializeField] private Transform[] waypoints = { };
    [SerializeField] private float sparkSpd = 1f;
    [SerializeField] private Transform sparkTransform;

    private bool moveSpark;
    private int index = 0;

    // Start is called before the first frame update
    void Awake()
    {
        system.Stop();
        moveSpark = false;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveSpark && waypoints.Length > 0 && index < waypoints.Length)
        {
            sparkTransform.position = Vector2.MoveTowards(sparkTransform.position, 
                                                    waypoints[index].position, 
                                                    sparkSpd * Time.deltaTime);
            if (sparkTransform.position == waypoints[index].position )
            {
                index++;
            }
        }
    }

    public void StartFuse()
    {
        system.Play();
        moveSpark = true;
        index = 0;
    }

    public void StopFuse()
    {
        system.Stop();
        moveSpark = false;
    }
}
