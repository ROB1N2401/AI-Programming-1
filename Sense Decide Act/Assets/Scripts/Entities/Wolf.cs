using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    public const int WOLF_MAX_HEALTH = 150;
    public const int WOLF_EATING_RATE = 50;

    private Sheep _targetedSheep;
    private List<Sheep> _sheepSeen;

    private void Awake()
    {
        entityType = EntityType.Wolf;
        currentHealth = WOLF_MAX_HEALTH * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);
        isHungry = false;
        isReadyToBreed = false;
        state = AnimalState.Wandering;
        _sheepSeen = new List<Sheep>();
        _targetedSheep = null;
        speed = 20;
        tileToWander = null;
        stateSprite = transform.GetComponent<SpriteRenderer>();
        healthSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        tileToWander = WorldGrid.Instance.GetRandomTile();
        Decide();
    }

    public override void Sense()
    {
        _sheepSeen = GetSheepInRadius(2);
    }

    public override void Decide()
    {
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
            _targetedSheep = GetSheepToPursue();
            state = _targetedSheep != null ? AnimalState.Pursuing : AnimalState.Wandering;
        }

        else
            state = AnimalState.Wandering;
        

        UpdateStateColor();
    }

    #region FSM
    void Update()
    {
        occupiedTile = WorldGrid.Instance.WorldToTilePoint(transform.position);
        currentHealth -= HEALTH_DEPLETION_RATE * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, WOLF_MAX_HEALTH);
        UpdateHealthColor(WOLF_MAX_HEALTH);
        ClampPosition();

        if (currentHealth == 0)
            Die();

        switch (state)
        {
            case AnimalState.Eating:
                Eat(_targetedSheep, WOLF_EATING_RATE);
                break;
            case AnimalState.Breeding:
                Breed(WOLF_MAX_HEALTH);
                Decide();
                break;
            case AnimalState.Pursuing:
                MoveTowards(_targetedSheep);
                break;
            case AnimalState.Wandering:
                MoveTowards(tileToWander);
                break;
        }
    }
    #endregion

    private List<Sheep> GetSheepInRadius(ushort tileRadius)
    {
        var sheepToReturn = new List<Sheep>();

        foreach (var sheep in Main.Instance.SheepCollection)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - sheep.Value.transform.position));

            if (tileRadius * WorldGrid.WORLD_STEP < distance) continue;
                sheepToReturn.Add(sheep.Value);
        }

        return sheepToReturn;
    }

    private Sheep GetSheepToPursue()
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
