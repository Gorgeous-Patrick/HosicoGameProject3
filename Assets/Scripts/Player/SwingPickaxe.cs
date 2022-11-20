using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingPickaxe : MonoBehaviour
{

  // the pivot to rotate around, defaults to parent if not set
  [SerializeField] Transform pivot;
  [SerializeField] float angularSpeed;
  [SerializeField] float offset = 120;
  [SerializeField] float swingRange = 90;

  void Start()
  {
    if (pivot == null) pivot = transform.parent;
  }

  void Update()
  {
    /*transform.RotateAround(pivot.position,
                           new Vector3(0, 0, 1),
                           (reversed ? -angularSpeed : angularSpeed) * Time.deltaTime
                          );*/
    transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * angularSpeed, swingRange) - offset);
  }

}
