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

  public void DestroyTileMapAtPoint(Vector3 Pos, int dmg)
  {
    ParticleSystemManager.RequestParticlesAtPositionAndDirection(Pos, Vector3.up);
    Vector3Int cellPos = destructibleTilemap.WorldToCell(Pos);
    TileBase dugTile = destructibleTilemap.GetTile(cellPos);
    if (dugTile != null)
    {
      // Debug.Log("health of tile: " + tilemapDataDict[cellPos.ToString()]);
      if (tilemapDataDict[cellPos.ToString()] > dmg)
      {
        tilemapDataDict[cellPos.ToString()] -= dmg;
        destructibleTilemap.SetTile(cellPos, crackedTile);
      }
      else destructibleTilemap.SetTile(cellPos, null);
    }
  }

}
