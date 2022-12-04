using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{

  static ScreenShakeManager instance;

  void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(gameObject);
      return;
    }
    else
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  public static void perturb()
  {
    instance.transform.localPosition = UnityEngine.Random.onUnitSphere * instance.amplitude;
  }

  [SerializeField] float amplitude = 0.5f;
  [SerializeField] float k = 0.3f;
  [SerializeField] float dampening_factor = 0.95f;
  Vector3 velocity = Vector3.zero;

  void Update()
  {
    Vector3 displacement = Vector3.zero - transform.localPosition;
    Vector3 acceleration = k * displacement;
    velocity += acceleration;
    velocity *= dampening_factor;
    transform.localPosition += velocity;
  }

}
