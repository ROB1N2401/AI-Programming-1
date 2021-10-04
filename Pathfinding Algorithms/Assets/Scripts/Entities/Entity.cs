using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour
{
    private Tile _gridPos;

    public Tile GridPos { get => _gridPos; set => _gridPos = value; }

    public void SpawnEntity()
    {
        var x = WorldGrid.Instance.GridSizeX - 1;
        var y = WorldGrid.Instance.GridSizeY - 1;

        while (true)
        {
            var i = Random.Range(0, x);
            var j = Random.Range(0, y);

            var tile = WorldGrid.Instance.TileStorage[i, j];
            if (tile.Walkable)
            {
                GridPos = tile;
                this.transform.position = GridPos.transform.position;
                break;
            }
        }
    }
}
