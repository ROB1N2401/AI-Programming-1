using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    public const int WOLF_MAX_HEALTH = 150;

    private const int WOLF_EATING_RATE = Sheep.SHEEP_MAX_HEALTH / 2;
    private const int WOLF_WALKING_SPEED = 14;
    private const int WOLF_RUNNING_SPEED = WOLF_WALKING_SPEED * 2;
    private const int WOLF_HEALTH_DEPLETION_RATE = WOLF_MAX_HEALTH / 20;

    private Sheep _targetedSheep;
    private List<Sheep> _sheepSeen;

    private void Awake()
    {
        stateSprite = transform.GetComponent<SpriteRenderer>();
        healthSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        isHungry = false;
        isReadyToBreed = false;
        entityType = EntityType.Wolf;
        currentHealth = WOLF_MAX_HEALTH * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);

        _sheepSeen = new List<Sheep>();
        _targetedSheep = null;
    }

    public override void Sense()
    {
        _sheepSeen = GetSheepInRadius(2);
    }

    public override void Decide()
    {
        var wasBreeding = state is AnimalState.Breeding;

        isReadyToBreed = (currentHealth > WOLF_MAX_HEALTH * BREEDING_COEFFICIENT);
        isHungry = (currentHealth < WOLF_MAX_HEALTH * HUNGER_COEFFICIENT);
        if (occupiedTile == tileToWander)
            tileToWander = WorldGrid.Instance.GetRandomTile();

        if (_targetedSheep != null && occupiedTile == _targetedSheep.OccupiedTile)
            state = AnimalState.Eating;

        else if(state is AnimalState.Pursuing && _targetedSheep != null) //Wolf will ignore everything until he hunts down the sheep he decided to hunt
            return;

        else if (isReadyToBreed)
            state = AnimalState.Breeding;

        else if (isHungry)
        {
            _targetedSheep = GetRandomSheepToPursue();
            state = _targetedSheep != null ? AnimalState.Pursuing : AnimalState.Wandering;
        }

        else
            state = AnimalState.Wandering;
        
        if(!wasBreeding)
            UpdateStateColor();
    }

    #region FSM
    void Update()
    {
        occupiedTile = WorldGrid.Instance.WorldToTilePoint(transform.position);
        currentHealth -= WOLF_HEALTH_DEPLETION_RATE * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, WOLF_MAX_HEALTH);
        UpdateHealthColor(WOLF_MAX_HEALTH);
        ClampPosition();

        if (currentHealth == 0)
            Die();

        switch (state)
        {
            case AnimalState.Eating:
                Eat(_targetedSheep, WOLF_EATING_RATE, WOLF_RUNNING_SPEED);
                break;
            case AnimalState.Breeding:
                Breed(WOLF_MAX_HEALTH);
                Decide();
                break;
            case AnimalState.Pursuing:
                MoveTowards(_targetedSheep, WOLF_RUNNING_SPEED);
                break;
            case AnimalState.Wandering:
                MoveTowards(tileToWander, WOLF_WALKING_SPEED);
                break;
        }
    }
    #endregion

    private List<Sheep> GetSheepInRadius(ushort tileRadius)
    {
        var sheepToReturn = new List<Sheep>();
        var allSheep = Main.GetEntitiesOfType<Sheep>();

        foreach (var sheep in allSheep)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - sheep.transform.position));

            if (tileRadius * WorldGrid.WORLD_STEP < distance) continue;
                sheepToReturn.Add(sheep);
        }

        return sheepToReturn;
    }

    private Sheep GetRandomSheepToPursue()
    {
        _sheepSeen = GetSheepInRadius(2);
        return _sheepSeen.Count == 0 ? null : _sheepSeen[Random.Range(0, _sheepSeen.Count)];

    }

    private Sheep GetNearestSheepToPursue()
    {
        _sheepSeen = GetSheepInRadius(2);
        if (_sheepSeen.Count == 0)
            return null;

        Sheep sheepToReturn = null;
        var shortestDistance = 10000.0f;

        foreach (var sheep in _sheepSeen)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - sheep.transform.position));

            if (distance > shortestDistance) continue;

            shortestDistance = distance;
            sheepToReturn = sheep;
        }

        return sheepToReturn;
    }
}
