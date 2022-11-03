using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public GameObject player;
  public Vector3 offset;
  void Update()
  {
    transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0) + offset;
  }
}
