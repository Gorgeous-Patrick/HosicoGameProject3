using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBarControl : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    float cellWidth;
    // Start is called before the first frame update
    void Start()
    {
        int maxBattery = Gameplay.maxBattery;
        for (int i = 0; i < maxBattery; i++)
        {
            Debug.Log("Creating Battery Cells");
            GameObject battery = Instantiate(Resources.Load("Prefabs/BatteryBarCell") as GameObject, transform);
            battery.GetComponent<BatteryCellControl>().batteryCellId = i;
            cellWidth = battery.GetComponent<RectTransform>().rect.width + 5;
            battery.transform.localPosition = new Vector3(offset.x + cellWidth * i, offset.y, 0);
        }
    }
}
