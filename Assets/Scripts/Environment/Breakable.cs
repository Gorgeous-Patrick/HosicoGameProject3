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
    GameObject other = collisionInfo.gameObject;
    if (other.tag == "Player")
      Destroy(gameObject);
  }

}
