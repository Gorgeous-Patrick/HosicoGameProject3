using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWaverScript : MonoBehaviour
{
    [SerializeField] Transform icon;

    [SerializeField] float speed = 5f;
    [SerializeField] float height = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = icon.position;
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        //set the object's Y to the new calculated Y
        icon.position = new Vector3(pos.x, newY, 1);
    }
}
