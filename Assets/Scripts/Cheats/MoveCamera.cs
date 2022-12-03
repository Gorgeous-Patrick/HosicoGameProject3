using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    GameObject MovableCamera;
    [SerializeField] float offsetValue = 1.0f;

    private void Start() {
        MovableCamera = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            MovableCamera.transform.position = new Vector3(MovableCamera.transform.position.x, MovableCamera.transform.position.y + offsetValue, MovableCamera.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            MovableCamera.transform.position = new Vector3(MovableCamera.transform.position.x, MovableCamera.transform.position.y - offsetValue, MovableCamera.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            MovableCamera.transform.position = new Vector3(MovableCamera.transform.position.x - offsetValue, MovableCamera.transform.position.y, MovableCamera.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            MovableCamera.transform.position = new Vector3(MovableCamera.transform.position.x + offsetValue, MovableCamera.transform.position.y, MovableCamera.transform.position.z);
        }
    }
}
