using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    void Start () {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();

        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null) {
                    Debug.Log("NONNULLx:" + x + " y:" + y + " tile:" + tile.name);
                    Vector3Int coord = new Vector3Int(x, y);
                    Vector3 coordWorld = gridLayout.CellToWorld(coord);
                    Debug.Log("coordWorld:" + coordWorld);
                } 
            
            }        
        }   
    }
}
