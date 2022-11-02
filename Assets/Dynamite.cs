using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public float explodeDelay = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplodeAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ExplodeAfterTime() {
        yield return new WaitForSeconds(explodeDelay);
    }
}
