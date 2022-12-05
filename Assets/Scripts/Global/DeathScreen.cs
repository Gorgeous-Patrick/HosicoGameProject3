using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
  TextMeshProUGUI healthText;
  Animator anim;
  GameObject image;

  void Start()
  {
    image = transform.Find("Image").gameObject;
    GameObject text = image.transform.Find("Text").gameObject;
    healthText = text.GetComponent<TextMeshProUGUI>();
    anim = text.GetComponent<Animator>();
    EventBus.Subscribe<EventUpdateHealth>((e) => healthText.text = $"x {e.newHealth}");
    EventBus.Subscribe<EventShowHealthUI>(updateContent);
  }

  void updateContent(EventShowHealthUI e)
  {
    if (e.isSuicide)
    {
      image.GetComponent<Image>().enabled = false;
      healthText.text = "Resetting";
    }
    if (e.isZero)
    {
      anim.SetTrigger("red");
    }
  }

}
