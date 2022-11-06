using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SticksToWorldPos : MonoBehaviour
{
  public Vector3 worldPos;
  void Update() =>
    GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(worldPos);
}
