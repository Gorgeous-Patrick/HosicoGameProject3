using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersHelp : MonoBehaviour
{
    [SerializeField] GameObject HelpPanel;

    public void ToggleHelp() {
        if (HelpPanel != null) {
            HelpPanel.SetActive(!HelpPanel.activeSelf);
        }
    }
}