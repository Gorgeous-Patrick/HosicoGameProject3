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
        StartCoroutine(BreakingTileDelay(cellPos));
    }

    private IEnumerator BreakingTileDelay(Vector3Int cellPos)
    {
        yield return new WaitForSeconds(0.12f);
        destructibleTilemap.SetTile(cellPos, null);
    }
}
