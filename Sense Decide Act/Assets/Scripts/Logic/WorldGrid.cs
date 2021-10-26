using System.Collections.Generic;
using Support;
using UnityEngine;

public class WorldGrid : MonoSingleton<WorldGrid>
{
    public const int WORLD_STEP = 5; //distance between each tile in world coordinate units
    public const int GRID_SIZE_X = 10;
    public const int GRID_SIZE_Y = 10;
    public const int WORLD_BORDER_LEFT = 0;
    public const int WORLD_BORDER_TOP = 0;
    public const int WORLD_BORDER_RIGHT = (GRID_SIZE_X - 1) * WORLD_STEP;
    public const int WORLD_BORDER_BOTTOM = -(GRID_SIZE_Y - 1) * WORLD_STEP;

    private const float GRASS_SPAWN_CHANCE = 0.8f;

    private Tile[,] _tileStorage;
    private Vector2 _gridWorldSize;

    public Tile[,] TileStorage => _tileStorage;

    public void CreateGrid()
    {
        _tileStorage = new Tile[GRID_SIZE_X, GRID_SIZE_Y];

        for (var i = 0; i < GRID_SIZE_X; i++)
        {
            for (var j = 0; j < GRID_SIZE_Y; j++)
            {
                var pos = new Vector3(i * WORLD_STEP, -j * WORLD_STEP);
                var go = Instantiate(Resources.Load("Tile", typeof(GameObject)), this.gameObject.transform) as GameObject;
                if (go is null)
                {
                    Debug.LogError("Failed to initialize a tile");
                    return;
                }
            
                go.transform.position = pos;
                go.name = $"Tile [{i}] [{j}]";
                var tile = go.GetComponent<Tile>();
                var hasGrass = Random.Range(0f, 1f) <= GRASS_SPAWN_CHANCE;
                tile.Initialize(hasGrass, pos, i, j);
                TileStorage[i, j] = tile;
            }
        }

        _gridWorldSize = TileStorage[GRID_SIZE_X - 1, GRID_SIZE_Y - 1].WorldPos;
    }

    public List<Tile> GetNeighbourTiles(Tile targetTile, ushort radius)
    {
        var neighbourTiles = new List<Tile>();

        for (var x = -radius; x <= radius; x++)
        {
            for (var y = -radius; y <= radius; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                var neighbourX = targetTile.GridPosX + x;
                var neighbourY = targetTile.GridPosY + y;

                if (neighbourX >= 0 && neighbourX < GRID_SIZE_X && neighbourY >= 0 && neighbourY < GRID_SIZE_Y)
                {
                    neighbourTiles.Add(TileStorage[neighbourX, neighbourY]);
                }
            }
        }

        return neighbourTiles;
    }

    public Tile WorldToTilePoint(Vector2 worldPosition)
    {
        var percentX = worldPosition.x / _gridWorldSize.x;
        var percentY = worldPosition.y / _gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        var x = Mathf.RoundToInt((GRID_SIZE_X - 1) * percentX);
        var y = Mathf.RoundToInt((GRID_SIZE_Y - 1) * percentY);

        return _tileStorage[x, y];
    }

    public Tile GetRandomTile()
    {
        var x = Random.Range(0, GRID_SIZE_X);
        var y = Random.Range(0, GRID_SIZE_Y);

        return _tileStorage[x, y];
    }
}
