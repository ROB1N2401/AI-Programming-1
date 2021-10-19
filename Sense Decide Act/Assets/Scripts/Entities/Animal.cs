using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AnimalState
{
    Eating,
    Breeding,
    Pursuing,
    Wandering,
    Evading
}

public abstract class Animal : Entity
{
    protected const int HEALTH_DEPLETION_RATE = 10;
    protected const float HUNGER_COEFFICIENT = 0.6f; //defines the threshold for animal to start feel hungry
    protected const float BREEDING_COEFFICIENT = 0.6f; //defines the threshold for animal to be ready for breeding

    protected bool isHungry;
    protected bool isReadyToBreed;
    protected int speed;
    protected AnimalState state;
    protected Tile tileToWander;
    protected SpriteRenderer stateSprite;
    protected SpriteRenderer healthSprite;

    public AnimalState State => state;
    public bool Ishungry => isHungry;

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

    protected void ClampPosition()
    {
        var position = transform.position;
        var x = Mathf.Clamp(position.x, 0.0f, (WorldGrid.Instance.GridSizeX - 1) * WorldGrid.WORLD_STEP);
        var y = Mathf.Clamp(position.y, -(WorldGrid.Instance.GridSizeY - 1) * WorldGrid.WORLD_STEP, 0.0f);
        transform.position = new Vector3(x, y, position.z);
    }

    protected void UpdateStateColor()
    {
        switch (state)
        {
            case AnimalState.Evading:
                stateSprite.color = Color.yellow;
                break;

            case AnimalState.Eating:
                stateSprite.color = Color.cyan;
                break;

            case AnimalState.Breeding:
                stateSprite.color = ColorLibrary.pink;
                break;

            case AnimalState.Pursuing:
                stateSprite.color = ColorLibrary.orange;
                break;

            case AnimalState.Wandering:
                stateSprite.color = Color.white;
                break;
        }
    }

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
        transform.position = Vector3.MoveTowards(transform.position, entity.transform.position, speed * Time.deltaTime);
    }

    protected void Eat(Entity entity, int eatingRate)
    {
        if(entityType is EntityType.Sheep && entity.OccupiedTile.GrassComponent.enabled == false)
            return;

        if (entity == null)
        {
            Decide(); //since entity that animal was trying to eat is null, it means it has died and animal needs to reevaluate its behaviour
            return;
        }

        MoveTowards(entity);
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
