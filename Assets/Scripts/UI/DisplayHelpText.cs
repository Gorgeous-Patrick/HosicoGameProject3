using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class DisplayHelpText : MonoBehaviour
{

  [SerializeField] Sprite icon;
  [SerializeField] string text;
  [SerializeField] Vector2 offset;
  GameObject hintObj;

  void Start()
  {
    hintObj = null;
  }

  void OnTriggerEnter2D(Collider2D triggerInfo)
  {
    if (triggerInfo.gameObject.tag != "Player") return;
    Destroy(hintObj);
    hintObj = Resources.Load("Prefabs/Hint") as GameObject;
    GameObject hintIcon = hintObj.transform.Find("Icon").gameObject;
    GameObject hintText = hintObj.transform.Find("Text").gameObject;
    hintIcon.GetComponent<Image>().sprite = icon;
    hintText.GetComponent<TextMeshProUGUI>().text = text;
    hintObj.GetComponent<SticksToWorldPos>().worldPos =
      Utils.flatten(transform.position)
      + new Vector2(0, transform.localScale.y / 2)
      + offset;
    hintObj = Instantiate(hintObj, GameObject.Find("Canvas").transform);
  }

  void OnTriggerExit2D(Collider2D triggerInfo)
  {
    if (triggerInfo.gameObject.tag != "Player") return;
    Destroy(hintObj);
    hintObj = null;
  }

}
