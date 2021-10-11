using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Grass))]
public class Tile : MonoBehaviour
{
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    [HideInInspector] public Tile parent;

    private int _gridPosX;
    private int _gridPosY;
    private bool _isAlive;
    private Vector3 _worldPos;

    public bool IsAlive => _isAlive;
    public int GridPosX => _gridPosX;
    public int GridPosY => _gridPosY;
    public Vector3 WorldPos => _worldPos;
    public int FCost => gCost + hCost;

    //Method carries a role of constructor, hence it's positioned above other methods
    public void Initialize(bool isAlive, Vector3 worldPos, int gridPosX, int gridPosY)
    {
        _isAlive = isAlive;
        _worldPos = worldPos; 
        _gridPosX = gridPosX;
        _gridPosY = gridPosY;

        GetComponent<Grass>().enabled = _isAlive;
        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = !_isAlive ? new Color(0.3f, 0.1f, 0.0f) : Color.green;
    }

    /// <summary>
    /// Checks whether the tile is occupied or not
    /// </summary>
    /// <param name="tile">tile to check</param>
    /// <returns>true if tile is occupied by a single entity</returns>
    public static bool CheckIfTileIsOccupied(Tile tile)
    {
        //var entitiesList = Main.Instance.Entities;

        return true; //entitiesList.Any(t => t.Value.OccupiedTile == tile);
    }
}
