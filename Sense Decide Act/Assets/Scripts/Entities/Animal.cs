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
    protected const float HUNGER_COEFFICIENT = 0.6f; //defines the threshold for animal to start feel hungry
    protected const float BREEDING_COEFFICIENT = 0.8f; //defines the threshold for animal to be ready for breeding

    protected bool isHungry;
    protected bool isReadyToBreed;
    protected AnimalState state;
    protected Tile tileToWander;
    protected SpriteRenderer stateSprite;
    protected SpriteRenderer healthSprite;

    public AnimalState State => state;
    public bool IsHungry => isHungry;

    protected void Start()
    {
        tileToWander = WorldGrid.Instance.GetRandomTile();
        Decide();
    }

    private void SetAnimalsPosition(Tile tile)
    {
        occupiedTile = tile;
        transform.position = occupiedTile.transform.position;
    }

    protected void ClampPosition()
    {
        var position = transform.position;
        var x = Mathf.Clamp(position.x, WorldGrid.WORLD_BORDER_LEFT, WorldGrid.WORLD_BORDER_RIGHT);
        var y = Mathf.Clamp(position.y, WorldGrid.WORLD_BORDER_BOTTOM, WorldGrid.WORLD_BORDER_TOP);
        transform.position = new Vector3(x, y, position.z);
    }

    protected void MoveTowards(Tile tile, int speed)
    {
        if(tile == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, tile.WorldPos, speed * Time.deltaTime);
    }

    protected void MoveTowards(Entity entity, int speed)
    {
        if(entity == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, entity.transform.position, speed * Time.deltaTime);
    }

    protected override void UpdateHealthColor(int maxHealth)
    {
        var healthLeft = currentHealth / maxHealth;
        healthSprite.color = new Color(1f - healthLeft, healthLeft, 0f);
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

    protected void Eat(Entity entity, int eatingRate, int speed)
    {
        if(entityType is EntityType.Sheep && entity.OccupiedTile.GrassComponent.enabled == false)
            return;

        if (entity == null)
        {
            Decide(); //since entity that animal was trying to eat is null, it means it has died and animal needs to reevaluate its behaviour
            return;
        }

        MoveTowards(entity, speed);
        var deltaHealth = eatingRate * Time.deltaTime;
        entity.CurrentHealth -= deltaHealth;
        currentHealth += deltaHealth;
    }

    protected void Breed(int maxHealth)
    {
        var startingHealth = maxHealth * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);
        currentHealth -= startingHealth;

        var newAnimal = (Animal)Main.Instantiate(entityType);
        newAnimal.SetAnimalsPosition(GetNearestFreeTile(this));
        newAnimal.currentHealth = startingHealth;
    }

    protected override void Die()
    {
        Main.Instance.EntityCollection.Remove(transform.gameObject.GetInstanceID());
        Destroy(gameObject);
    }

    public void PlaceAnimalOnRandomTile()
    {
        const int x = WorldGrid.GRID_SIZE_X - 1;
        const int y = WorldGrid.GRID_SIZE_Y - 1;

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
}
