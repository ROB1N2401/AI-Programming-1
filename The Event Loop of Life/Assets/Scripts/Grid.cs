using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 GridWorldSize;
    public float TileRadius;
    public Tile[,] GridStorage;
    private float _tileDiameter;
    private int _gridSizeX, _gridSizeY; //amount of tiles fit within GridWorldSize

    void Awake()
    {
        GridWorldSize = new Vector2(15, 15);
        TileRadius = 0.5f;

        _tileDiameter = TileRadius * 2;

        _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _tileDiameter);
        _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _tileDiameter);
    }

    void Start()
    {
        //CreateGrid();
    }

    public List<Tile> GetNeighbourTiles(Tile targetTile_in)
    {
        List<Tile> neighbourTiles_ = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                {
                    continue;
                }

                int neighbourX = targetTile_in.GridPosX + x;
                int neighbourY = targetTile_in.GridPosY + y;

                if (neighbourX >= 0 && neighbourX < _gridSizeX && neighbourY >= 0 && neighbourY < _gridSizeY)
                {
                    neighbourTiles_.Add(GridStorage[neighbourX, neighbourY]);
                }
            }
        }

        return neighbourTiles_;
    }

    public void CreateGrid(GameObject parent_in)
    {
        GridStorage = new Tile[_gridSizeX, _gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * (GridWorldSize.x / 2)) - (Vector3.up * (GridWorldSize.y / 2));

        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * _tileDiameter + TileRadius)) + (Vector3.up * (y * _tileDiameter + TileRadius));
  
                GridStorage[x, y] = new Tile(worldPoint, parent_in, x, y);
            }
        }
    }

    public Tile WorldToTilePoint(Vector2 worldPosition_in)
    {
        float percentX = (worldPosition_in.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition_in.y + GridWorldSize.y / 2) / GridWorldSize.y;

        Mathf.Clamp01(percentX);
        Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);

        return GridStorage[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, GridWorldSize.y, 1));

        if (GridStorage != null)
        {
            foreach (Tile n in GridStorage)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(n.WorldPos, Vector3.one * (_tileDiameter - 0.1f));
            }
        }
    }
}
