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
    protected float currentHealth;

    public Tile OccupiedTile { get => occupiedTile; set => occupiedTile = value; }

    public abstract void Sense();
    public abstract void Decide();

    protected abstract void Die();
}
