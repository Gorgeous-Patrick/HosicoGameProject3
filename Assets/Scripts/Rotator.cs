using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // using Time.deltaTime gets the seconds as a float since the last frame update, so it changes dynamically depending on the length of a frame
        // aka frame independent, so it's changes per second rather than per frame
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
