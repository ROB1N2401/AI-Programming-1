using System.Linq;
using UnityEngine;

public enum EntityType
{
    Grass,
    Animal
}

public abstract class Entity : MonoBehaviour
{
    protected EntityType entityType;
    protected Tile occupiedTile;
    [SerializeField] protected float currentHealth;

    public Tile OccupiedTile { get => occupiedTile; set => occupiedTile = value; }

    public abstract void Sense();
    public abstract void Decide();

    protected abstract void UpdateHealthColor(int maxHealth);
    protected abstract void Die();

    protected Tile GetNearestFreeTile()
    {
        var neighbourTiles = WorldGrid.Instance.GetNeighbourTiles(occupiedTile);

        switch (entityType)
        {
            case EntityType.Animal:
                return neighbourTiles.FirstOrDefault(tile => !Tile.CheckIfTileIsOccupiedByAnimal(tile));
                
            case EntityType.Grass:
                return neighbourTiles.FirstOrDefault(tile => !Tile.CheckIfTileHasGrass(tile));
        }

        Debug.LogWarning("There is no free tile nearby");
        return null;
    }
}
