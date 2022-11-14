using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MinableTile : MonoBehaviour
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
        ParticleSystemManager.RequestParticlesAtPositionAndDirection(cellPos, Vector3.up);
        yield return new WaitForSeconds(0.28f);
        destructibleTilemap.SetTile(cellPos, null);
    }
}
