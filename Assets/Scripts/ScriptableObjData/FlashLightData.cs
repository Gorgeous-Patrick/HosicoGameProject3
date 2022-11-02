using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShellData", menuName = "Data/ShellData")]

public class ShellData : ScriptableObject
{
    public int dmg = 5;
    public float shellSpd = 70;
    public float shellMaxRange = 120;
    public float penetrationVal = 50;
}
