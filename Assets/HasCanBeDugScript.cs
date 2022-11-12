using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HasCanBeDugScript : MonoBehaviour
{
    public Tilemap destructibleTilemap;

    // Start is called before the first frame update
    void Awake()
    {
        destructibleTilemap = GetComponent<Tilemap>();
    }

    public void DestroyTileMapAtPoint( Vector3 Pos)
    {
        Vector3Int cellPos = destructibleTilemap.WorldToCell(Pos);
        destructibleTilemap.SetTile(cellPos, null);
    }
}
