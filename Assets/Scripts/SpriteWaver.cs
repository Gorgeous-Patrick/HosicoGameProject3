using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWaver : MonoBehaviour
{
  [SerializeField] float speed = 5f;
  [SerializeField] float height = 0.5f;

  float y;

  void Start()
  {
    y = transform.position.y;
  }

  void Update()
  {
    transform.position = new Vector3(transform.position.x, y + Mathf.Abs(Mathf.Sin(Time.time * speed) * height));
  }
}
