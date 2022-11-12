using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBar : MonoBehaviour
{
    private float fullDistance;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        GameObject phone = Gameplay.phone;
        GameObject player = Gameplay.player;
        fullDistance = Vector2.Distance(player.transform.position, phone.transform.position);
        Debug.Log(fullDistance);
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject phone = Gameplay.phone;
        GameObject player = Gameplay.player;
        float currentDistance = Vector2.Distance(player.transform.position, phone.transform.position);
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
