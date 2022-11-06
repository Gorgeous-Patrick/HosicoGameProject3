using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{

  // The function that gets called when the player presses the E key
  protected abstract void Interact();

  // The function get gets called when the player leaves
  protected virtual void Interrupt() {}

  void OnTriggerEnter2D(Collider2D triggerInfo)
  {
    if (triggerInfo.gameObject.tag != "Player") return;
    Gameplay.funcInteract += Interact;
  }

  void OnTriggerExit2D(Collider2D triggerInfo)
  {
    if (triggerInfo.gameObject.tag != "Player") return;
    Gameplay.funcInteract -= Interact;
    Interrupt();
  }

}
