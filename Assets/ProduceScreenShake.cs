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
    Vector3 shakeDirection = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * new Vector3(shakeStrength, 0, 0);
    cineShake.GenerateImpulseWithVelocity(shakeDirection);
  }
}
