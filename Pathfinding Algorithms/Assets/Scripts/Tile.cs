using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int gCost;
    [HideInInspector] public int hCost;
    public int gridPosX;
    public int gridPosY;
    public bool walkable;
    public Vector3 worldPos;
    [HideInInspector] public Tile parent;

    public int FCost => gCost + hCost;

    //Method carries a role of constructor, hence it's positioned above other methods
    public void Initialize(bool walkableIn, Vector3 worldPosIn, int gridPosXIn, int gridPosYIn)
    {
        walkable = walkableIn;
        worldPos = worldPosIn;
        gridPosX = gridPosXIn;
        gridPosY = gridPosYIn;

        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = !walkable ? new Color(0.25f, 0.25f, 0.25f) : Color.white;
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
            if (t.Value.GridPos == tile)
                return true;
        }

        return false;
    }

    public void SwitchTileType()
    {
        if(CheckIfTileIsOccupied(this))
            return;

        walkable = !walkable;
        UpdateColor();
    }
}
