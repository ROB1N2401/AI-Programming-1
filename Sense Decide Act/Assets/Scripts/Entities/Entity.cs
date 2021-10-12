using UnityEngine;

public enum EntityType
{
    Grass,
    Sheep,
    Wolf
}

public abstract class Entity : MonoBehaviour
{
    protected EntityType entityType;
    protected Tile occupiedTile;

    public Tile OccupiedTile { get => occupiedTile; set => occupiedTile = value; }

    public static void Instantiate(string resourcePrefabName, EntityType entityType)
    {
        if (!(Instantiate(Resources.Load(resourcePrefabName, typeof(GameObject))) is GameObject go))
        {
            Debug.LogError($"Failed to instantiate entity {resourcePrefabName}");
            return;
        }

        var entityComponent = go.GetComponent<Entity>();

        switch (entityType)
        {
            case EntityType.Grass:
                Main.Instance.GrassCollection.Add(entityComponent.gameObject.GetInstanceID(), entityComponent.GetComponent<Grass>());
                break;
            case EntityType.Sheep:
                Main.Instance.SheepCollection.Add(entityComponent.gameObject.GetInstanceID(), entityComponent.GetComponent<Sheep>());
                break;
            case EntityType.Wolf:
                Main.Instance.WolvesCollection.Add(entityComponent.gameObject.GetInstanceID(), entityComponent.GetComponent<Wolf>());
                break;
            default:
                Debug.LogError("entityType has not been assigned");
                break;
        }
        entityComponent.PlaceEntityOnRandomTile();
    }

    public abstract void Sense();
    public abstract void Decide();

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

            if (entityType == EntityType.Grass)
            {
                if (!tile.IsAlive) continue;
            }
            else if (Tile.CheckIfTileIsOccupied(tile)) continue;

            SetEntitysPosition(tile);
            break;
        }
    }

    public void Die()
    {
        switch (entityType)
        {
            case EntityType.Grass:
                GetComponent<Tile>().SwitchTileState();
                Main.Instance.GrassCollection.Remove(transform.GetInstanceID());
                break;
            case EntityType.Sheep:
                Main.Instance.SheepCollection.Remove(transform.GetInstanceID());
                break;
            case EntityType.Wolf:
                Main.Instance.WolvesCollection.Remove(transform.GetInstanceID());
                break;
            default:
                Debug.LogError($"Game Object with ID {transform.GetInstanceID()} has not been found");
                break;
        }
    }
}
