using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWaver : MonoBehaviour
{
  [SerializeField] float speed = 5f;
  [SerializeField] float height = 0.5f;

  void Update()
  {
    Vector2 pos = transform.position;
    float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
    //set the object's Y to the new calculated Y
    transform.position = new Vector3(pos.x, newY, 1);
  }
}
