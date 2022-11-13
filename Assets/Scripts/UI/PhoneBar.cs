using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneBar : MonoBehaviour
{
    private float fullDistance;
    private Image img;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        GameObject phone = Gameplay.phone;
        GameObject player = Gameplay.player;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject phone = Gameplay.phone;
        GameObject player = Gameplay.player;
        float currentDistance = Vector2.Distance(player.transform.position, phone.transform.position);
        int ratioDistance = (int)currentDistance;
        text.text = ratioDistance.ToString() + "m";

    }
}
