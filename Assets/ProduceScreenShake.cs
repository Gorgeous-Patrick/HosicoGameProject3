using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ProduceScreenShake : MonoBehaviour
{
    [SerializeField] float shakeStrength = 1f;

    CinemachineImpulseSource cineShake;
    // Start is called before the first frame update
    void Awake()
    {
        cineShake = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        cineShake.GenerateImpulse(shakeStrength);
    }
}
