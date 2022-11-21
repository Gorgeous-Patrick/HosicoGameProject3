using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LavaLight : MonoBehaviour
{
    public float period = 1.0f;
    public float amplitude = 1.0f;
    Light2D light2D;
    public float timer = 0.0f;
    float offset = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
        offset = light2D.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        light2D.intensity = Mathf.Sin(2 * Mathf.PI * timer / period) * amplitude + offset;
    }
}
