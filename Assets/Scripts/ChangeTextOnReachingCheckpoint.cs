using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextOnReachingCheckpoint : MonoBehaviour
{
    TextMeshProUGUI dialogue;
    [SerializeField] private GameObject parent;
    // Start is called before the first frame update
    void Awake()
    {
        EventBus.Subscribe<OnChangeGoal>(handler_change_text);
        dialogue = GetComponent<TextMeshProUGUI>();
    }

    void handler_change_text(OnChangeGoal e)
    {
        dialogue.text = "Now that I have the pickaxe, its time to get the dynamite";
        parent.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
