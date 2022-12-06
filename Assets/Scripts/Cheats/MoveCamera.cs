using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] float offsetValue = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + offsetValue, this.gameObject.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - offsetValue, this.gameObject.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - offsetValue, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + offsetValue, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
    }
}
