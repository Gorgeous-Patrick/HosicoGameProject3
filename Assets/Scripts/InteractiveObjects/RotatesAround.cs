using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatesAround : MonoBehaviour
{

  // the pivot to rotate around, defaults to parent if not set
  [SerializeField] Transform pivot;
  [SerializeField] float angularSpeed;
  public bool reversed = false;

  void Start()
  {
    if (pivot == null) pivot = transform.parent;
  }

  void Update()
  {
    transform.RotateAround(pivot.position,
                           new Vector3(0, 0, 1),
                           (reversed ? -angularSpeed : angularSpeed) * Time.deltaTime
                          );
  }

}
