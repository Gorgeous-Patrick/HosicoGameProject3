using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBar : MonoBehaviour
{
    public GameObject phone;
    private float fullDistance;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        fullDistance = Vector2.Distance(transform.position, phone.transform.position);
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = Vector2.Distance(transform.position, phone.transform.position);
        float ratio = 1 - currentDistance / fullDistance;
        if (ratio < 0)
        {
            ratio = 0;
        }
        else if (ratio > 1)
        {
            ratio = 1;
        }
        img.fillAmount = ratio;
    }
}
