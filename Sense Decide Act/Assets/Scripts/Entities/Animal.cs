using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Animal : Entity
{
    protected const int HEALTH_DEPLETION_RATE = 5;
    protected const float HUNGER_COEFFICIENT = 0.5f; //defines the threshold for animal to start feel hungry
    protected const float BREEDING_COEFFICIENT = 0.9f; //defines the threshold for animal to be ready for breeding

    protected bool isHungry;
    protected bool isReadyToBreed;
    protected int speed;
    protected Tile tileToWander;
    protected SpriteRenderer stateSprite;
    protected SpriteRenderer healthSprite;

    private void SetAnimalsPosition(Tile tile)
    {
        occupiedTile = tile;
        this.transform.position = occupiedTile.transform.position;
    }

    public void PlaceAnimalOnRandomTile()
    {
        var x = WorldGrid.Instance.GridSizeX - 1;
        var y = WorldGrid.Instance.GridSizeY - 1;

        while (true)
        {
            var i = Random.Range(0, x);
            var j = Random.Range(0, y);

            var tile = WorldGrid.Instance.TileStorage[i, j];

            if (Tile.CheckIfTileIsOccupiedByAnimal(tile)) continue;

            SetAnimalsPosition(tile);
            break;
        }
    }

    protected abstract void UpdateStateColor();

    protected void MoveTowards(Tile tile)
    {
        if(tile == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, tile.WorldPos, speed * Time.deltaTime);
    }

    protected void MoveTowards(Entity entity)
    {
        if(entity == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, entity.OccupiedTile.WorldPos, speed * Time.deltaTime);
    }

    protected void Eat(Entity entity, int eatingRate)
    {
        if(entityType is EntityType.Sheep && entity.OccupiedTile.GrassComponent.enabled == false)
            return;

        var deltaHealth = eatingRate * Time.deltaTime;
        entity.CurrentHealth -= deltaHealth;
        currentHealth += deltaHealth;
    }

    protected void Breed(int maxHealth)
    {
        float startingHealth = maxHealth * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);
        currentHealth -= startingHealth;

        var newAnimal = Main.Instantiate(entityType);
        newAnimal.SetAnimalsPosition(GetNearestFreeTile(this));
        newAnimal.currentHealth = startingHealth;
    }

    protected override void UpdateHealthColor(int maxHealth)
    {
        var healthLeft = currentHealth / maxHealth;
        healthSprite.color = new Color(1f - healthLeft, healthLeft, 0f);
    }

    protected override void Die()
    {
        switch (entityType)
        {
            case EntityType.Sheep:
                Main.Instance.SheepCollection.Remove(GetInstanceID());
                break;
            case EntityType.Wolf:
                Main.Instance.WolvesCollection.Remove(GetInstanceID());
                break;
        }
        Destroy(gameObject);
    }
}
