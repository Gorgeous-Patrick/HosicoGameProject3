using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
  void Start()
  {

  }

  void Update()
  {

  }

  void OnCollisionEnter(Collision collisionInfo)
  {
    Destroy(gameObject);
  }

}
