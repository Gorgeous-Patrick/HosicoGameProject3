using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopSmokeScreen : MonoBehaviour
{
    Subscription<EventDidPopSmoke> sub_EventDidPopSmoke;

    public UnityEvent<float> OnPopSmokeShow = new UnityEvent<float>();

    // [SerializeField] Transform tankRear;
    [SerializeField] GameObject SmokePrefab;
    [SerializeField] GameObject SmokeUIPrefab;
    [SerializeField] private float numSmoke = 3f;

    private bool isUIActive = false;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        sub_EventDidPopSmoke = EventBus.Subscribe<EventDidPopSmoke>(PopSmoke);
        OnPopSmokeShow?.Invoke(numSmoke);
        // EventBus.Publish(new EventChangeTxtSmoke(numSmoke));
    }

    private void Update()
    {
        if (isUIActive)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane + 1;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            SmokeUIPrefab.transform.localRotation = Quaternion.identity;
            SmokeUIPrefab.transform.position = mousePos;
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (numSmoke > 0)
                {
                    GameObject smokeArea = Instantiate(SmokePrefab);
                    smokeArea.transform.localRotation = Quaternion.identity;
                    smokeArea.transform.position = mousePos;
                    numSmoke--;
                    OnPopSmokeShow?.Invoke(numSmoke);
                    SmokeUIPrefab.SetActive(false);
                    isUIActive = false;
                }
            }
        }
    }

    private void PopSmoke(EventDidPopSmoke obj)
    {
        if (numSmoke > 0)
        {
            if (isUIActive == false)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane + 1;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                SmokeUIPrefab.SetActive(true);
                SmokeUIPrefab.transform.localRotation = Quaternion.identity;
                SmokeUIPrefab.transform.position = mousePos;
                isUIActive = true;
            }
            else
            {
                SmokeUIPrefab.SetActive(false);
                isUIActive = false;
            }
        }
    }
}
