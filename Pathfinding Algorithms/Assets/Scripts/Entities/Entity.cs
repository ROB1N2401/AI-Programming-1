using UnityEngine;

public enum EntityType
{
    Starchaser,
    Star,
    TradingPost,
    Spaceship
}

public class Entity : MonoBehaviour
{
    protected Tile occupiedTile;

    public Tile OccupiedTile { get => occupiedTile; set => occupiedTile = value; }

    /// <summary>
    /// Sets entity's position to selected tile, both on grid and in-world. Doesn't consider tie's type, so be careful
    /// </summary>
    /// <param name="tile"></param>
    public void SetEntitysPosition(Tile tile)
    {
        occupiedTile = tile;
        this.transform.position = occupiedTile.transform.position;
    }

    public void PlaceEntityOnRandomTile()
    {
        var x = WorldGrid.Instance.GridSizeX - 1;
        var y = WorldGrid.Instance.GridSizeY - 1;

        while (true)
        {
            var i = Random.Range(0, x);
            var j = Random.Range(0, y);

            var tile = WorldGrid.Instance.TileStorage[i, j];
            if (tile.IsWalkable && !Tile.CheckIfTileIsOccupied(tile))
            {
                SetEntitysPosition(tile);
                break;
            }
        }
    }

    public static void Instantiate(string resourcePrefabName, EntityType entityType)
    {
        if (Instantiate(Resources.Load(resourcePrefabName, typeof(GameObject))) is GameObject go)
        {
            var entityComponent = go.GetComponent<Entity>();
            entityComponent.PlaceEntityOnRandomTile();
            Main.Instance.Entities.Add(resourcePrefabName, entityComponent);
        }
    }
}
