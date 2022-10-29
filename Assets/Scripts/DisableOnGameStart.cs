using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGameStart : MonoBehaviour
{
  void Awake()
  {
    Destroy(gameObject);
  }
}
