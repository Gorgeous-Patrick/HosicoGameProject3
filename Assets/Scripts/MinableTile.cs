using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MinableTile : MonoBehaviour
{
    public Tilemap destructibleTilemap;

    // [SerializeField] private List<TilemapData> tilemapDataList;

    [SerializeField] private TileBase crackedTile;

    private Dictionary<string, int> tilemapDataDict;

    // Start is called before the first frame update
    void Awake()
    {
        destructibleTilemap = GetComponent<Tilemap>();
        tilemapDataDict = new Dictionary<string, int>();

        foreach (var position in destructibleTilemap.cellBounds.allPositionsWithin)
        {
            // Vector2 cellPos2D = new Vector2(position.x, position.y);
            tilemapDataDict.Add(position.ToString(), 2);
            //Debug.Log("tilemapPosition: " + cellPos2D.ToString());
        }

        /*foreach (var tilemapData in tilemapDataList)
        {
            foreach (var tile in tilemapData.tiles)
            {
                tilemapDataDict.Add(tile, tilemapData);
                tilemapData.tileHealth = 1;
            }
        }*/
    }

    public void DestroyTileMapAtPoint( Vector3 Pos)
    {
        ParticleSystemManager.RequestParticlesAtPositionAndDirection(Pos, Vector3.up);
        Vector3Int cellPos = destructibleTilemap.WorldToCell(Pos);
        StartCoroutine(BreakingTileDelay(cellPos));
    }

    private IEnumerator BreakingTileDelay(Vector3Int cellPos)
    {
        TileBase dugTile = destructibleTilemap.GetTile(cellPos);
        yield return new WaitForSeconds(0.08f);
        if (dugTile != null) 
        {
            // Debug.Log("health of tile: " + tilemapDataDict[cellPos.ToString()]);
            if (tilemapDataDict[cellPos.ToString()] > 0)
            {
                tilemapDataDict[cellPos.ToString()] -= 1;
                destructibleTilemap.SetTile(cellPos, crackedTile);
            }
            else destructibleTilemap.SetTile(cellPos, null);
        }
    }
}
