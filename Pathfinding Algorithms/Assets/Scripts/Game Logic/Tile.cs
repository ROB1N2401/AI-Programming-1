using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int gCost;
    public int hCost;
    public Tile parent;

    private int _gridPosX;
    private int _gridPosY;
    private bool _isWalkable;
    private Vector3 _worldPos;

    public bool IsWalkable => _isWalkable;
    public int GridPosX => _gridPosX;
    public int GridPosY => _gridPosY;
    public Vector3 WorldPos => _worldPos;
    public int FCost => gCost + hCost;

    //Method carries a role of constructor, hence it's positioned above other methods
    public void Initialize(bool isWalkable, Vector3 worldPos, int gridPosX, int gridPosY)
    {
        _isWalkable = isWalkable;
        _worldPos = worldPos; 
        _gridPosX = gridPosX;
        _gridPosY = gridPosY;

        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = !_isWalkable ? new Color(0.25f, 0.25f, 0.25f) : Color.white;
    }

    /// <summary>
    /// Checks whether the tile is occupied or not
    /// </summary>
    /// <param name="tile">tile to check</param>
    /// <returns>true if tile is occupied by a single entity</returns>
    public static bool CheckIfTileIsOccupied(Tile tile)
    {
        var entitiesList = Main.Instance.Entities;

        return entitiesList.Any(t => t.Value.OccupiedTile == tile);
    }

    public void SwitchTileType()
    {
        if(CheckIfTileIsOccupied(this))
            return;

        _isWalkable = !IsWalkable;
        UpdateColor();
    }
}
