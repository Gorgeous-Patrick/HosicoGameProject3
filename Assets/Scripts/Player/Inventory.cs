using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

  // key: item name
  // value: count
  Dictionary<string, int> inventory;

  // key: item name
  // value: function called when item is used
  Dictionary<string, System.Action> funcUseItem;

  [SerializeField] GameObject dynamitePrefab;

  void Awake()
  {
    inventory = new Dictionary<string, int>();
    funcUseItem = new Dictionary<string, System.Action>();
  }

  // for now we only have one type of item: dynamite
  // later we might want to implement item switching
  public string heldItem
  {
    get => "dynamite";
  }

  void Start()
  {
    EventBus.Subscribe<EventUpdateInventory>((EventUpdateInventory e) =>
    {
      updateItem(e.pickup, e.delta);
    });
    EventBus.Subscribe<EventUseItem>((EventUseItem e) =>
    {
      funcUseItem[heldItem].Invoke();
    });
    inventory["dynamite"] = 0;
    inventory["health"] = 5;
    funcUseItem["dynamite"] = useDynamite;
  }

  void updateItem(string item, int delta)
  {
    inventory[item] += delta;
    EventBus.Publish(new EventUpdateInventoryUI { item = item, newAmount = inventory[item] });
  }

  void useDynamite()
  {
    if (dynamitePrefab != null && inventory["dynamite"] > 0)
    {
      updateItem("dynamite", -1);
      Instantiate(dynamitePrefab, transform.position, Quaternion.identity);
    }
  }

}
