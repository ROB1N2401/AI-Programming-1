﻿using System.Collections.Generic;
using Support;
using UnityEngine;

public class WorldGrid : MonoSingleton<WorldGrid>
{
    private Tile[,] _tileStorage;
    private float _grassTileSpawnChance;
    private int _gridSizeX;
    private int _gridSizeY;
    private Vector2 _gridWorldSize;

    public int GridSizeX => _gridSizeX;
    public int GridSizeY => _gridSizeY;
    public Tile[,] TileStorage => _tileStorage;

    // Start is called before the first frame update
    private void Start()
    {
        _grassTileSpawnChance = 0.4f;
        _gridSizeX = 10;
        _gridSizeY = 10;
    }

    public void CreateGrid()
    {
        _tileStorage = new Tile[_gridSizeX, _gridSizeY];

        for (var i = 0; i < _gridSizeX; i++)
        {
            for (var j = 0; j < _gridSizeY; j++)
            {
                var pos = new Vector3(i * 5, -j * 5);
                var go = Instantiate(Resources.Load("Tile", typeof(GameObject)), this.gameObject.transform) as GameObject;
                if (go is null)
                {
                    Debug.LogError("Failed to initialize a tile");
                    return;
                }
            
                go.transform.position = pos;
                go.name = $"Tile [{i}] [{j}]";
                var tile = go.GetComponent<Tile>();
                var hasGrass = Random.Range(0f, 1f) <= _grassTileSpawnChance;
                tile.Initialize(hasGrass, pos, i, j);
                TileStorage[i, j] = tile;
            }
        }

        _gridWorldSize = TileStorage[_gridSizeX - 1, _gridSizeY - 1].WorldPos;
    }

    public List<Tile> GetNeighbourTiles(Tile targetTile)
    {
        var neighbourTiles = new List<Tile>();

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                var neighbourX = targetTile.GridPosX + x;
                var neighbourY = targetTile.GridPosY + y;

                if (neighbourX >= 0 && neighbourX < _gridSizeX && neighbourY >= 0 && neighbourY < _gridSizeY)
                {
                    neighbourTiles.Add(TileStorage[neighbourX, neighbourY]);
                }
            }
        }

        return neighbourTiles;
    }

    public Tile WorldToTilePoint(Vector2 worldPosition)
    {
        if (worldPosition.x < -2.5f || worldPosition.x > _gridWorldSize.x + 2.5f ||
            worldPosition.y > 2.5f || worldPosition.y < _gridWorldSize.y - 2.5f)
            return null;

        var percentX = worldPosition.x / _gridWorldSize.x;
        var percentY = worldPosition.y / _gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        var x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        var y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return TileStorage[x, y];
    }
}
