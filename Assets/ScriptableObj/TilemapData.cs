using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TilemapData : ScriptableObject
{
    public TileBase[] tiles;
    public int tileHealth = 1;
}
