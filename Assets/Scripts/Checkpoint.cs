using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{

  [SerializeField] int index;

  void OnTriggerEnter2D(Collider2D collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Player")
      CheckpointController.updateCheckpoint(index, transform.position);
  }

}
