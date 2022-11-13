using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowsAndShrinks : MonoBehaviour
{

  [SerializeField] float period = 1f;
  [SerializeField] float sizeMultiplier = 1.2f;

  float timer;
  Transform tsfm;
  Vector3 originalScale;

  float _curve(float x)
  {
    var a = (x <= 0.5) ? x : 1 - x;
    return 1 + a * (sizeMultiplier - 1);
  }

  void Start()
  {
    tsfm = GetComponent<Transform>();
    originalScale = tsfm.localScale;
    timer = 0;
  }

  void Update()
  {
    timer += Time.deltaTime;
    if (timer >= period) timer -= period;
    tsfm.localScale = originalScale * _curve(timer / period);
  }

}
