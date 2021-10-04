using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int GCost;
    [HideInInspector] public int HCost;
    public int GridPosX;
    public int GridPosY;
    public bool Walkable;
    public Vector3 WorldPos;
    [HideInInspector] public Tile Parent;

    public int FCost => GCost + HCost;

    //Method carries a role of constructor, hence it's positioned above other methods
    public void Initialize(bool walkableIn, Vector3 worldPosIn, int gridPosXIn, int gridPosYIn)
    {
        Walkable = walkableIn;
        WorldPos = worldPosIn;
        GridPosX = gridPosXIn;
        GridPosY = gridPosYIn;

        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = !Walkable ? new Color(0.25f, 0.25f, 0.25f) : Color.white;
    }

    /// <summary>
    /// Checks whether the tile is occupied or not
    /// </summary>
    /// <param name="tile">tile to check</param>
    /// <returns>true if tile is occupied by an individual entity</returns>
    public static bool CheckIfTileIsOccupied(Tile tile)
    {
        var entitiesList = Main.Instance.Entities;
        foreach (var t in entitiesList)
        {
            if (t.GridPos == tile)
                return true;
        }

        return false;
    }

    public void SwitchTileType()
    {
        if(CheckIfTileIsOccupied(this))
            return;

        Walkable = !Walkable;
        UpdateColor();
    }
}
