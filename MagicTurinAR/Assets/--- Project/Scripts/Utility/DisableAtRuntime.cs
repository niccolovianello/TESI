using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;

public class DisableAtRuntime : MonoBehaviour
{

    private UnityTile[] tiles;

    private void TileDisable() {
        tiles = GetComponentsInChildren<UnityTile>();

        foreach (UnityTile tile in tiles)
        {
            tile.MeshRenderer.enabled = false;
            //Debug.Log(tile.name);
        }
    }

    private bool TileCreated()
    {
        return tiles.Length > 0;
    }
    
    private IEnumerator CheckTileCreated()
    {
        if (TileCreated())
        {
            TileDisable();
        }

        yield return new WaitForSeconds(.1f);
    }
}
