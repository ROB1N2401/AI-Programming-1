using System.Linq;
using UnityEngine;

public enum EntityType
{
    Grass,
    Sheep,
    Wolf
}

public abstract class Entity : MonoBehaviour
{
    protected const float MIN_STARTING_HEALTH_COEFFICIENT = 0.3f;
    protected const float MAX_STARTING_HEALTH_COEFFICIENT = 0.5f;

    protected EntityType entityType;
    protected Tile occupiedTile;
    [SerializeField] protected float currentHealth;

    public Tile OccupiedTile { get => occupiedTile; set => occupiedTile = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

    public abstract void Sense();
    public abstract void Decide();

    protected abstract void UpdateHealthColor(int maxHealth);
    protected abstract void Die();

    protected Tile GetNearestFreeTile(Entity entity)
    {
        var neighbourTiles = WorldGrid.Instance.GetNeighbourTiles(occupiedTile, 1);

        if(entity is Sheep || entity is Wolf)
            return neighbourTiles.FirstOrDefault(tile => !Tile.CheckIfTileIsOccupiedByAnimal(tile));

        if(entity is Grass)
            return neighbourTiles.FirstOrDefault(tile => !Tile.CheckIfTileHasGrass(tile));

        Debug.LogWarning("There is no free tile nearby");
        return null;
    }
}
