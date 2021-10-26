using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sheep : Animal
{
    public const int SHEEP_MAX_HEALTH = 90;

    private const int SHEEP_EATING_RATE = Grass.GRASS_MAX_HEALTH / 1;
    private const int SHEEP_WALKING_SPEED = 11;
    private const int SHEEP_RUNNING_SPEED = SHEEP_WALKING_SPEED * 2;
    private const int SHEEP_HEALTH_DEPLETION_RATE = SHEEP_MAX_HEALTH / 15;

    private Grass _grassToEat;
    private Vector2 _evadeDirection;
    private List<Wolf> _hungryWolvesSeen;
    private List<Grass> _grassSeen;

    private void Awake()
    {
        stateSprite = transform.GetComponent<SpriteRenderer>();
        healthSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        isHungry = false;
        isReadyToBreed = false;
        entityType = EntityType.Sheep;
        currentHealth = SHEEP_MAX_HEALTH * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);

        _grassToEat = null;
        _grassSeen = new List<Grass>();
        _hungryWolvesSeen = new List<Wolf>();
        _evadeDirection = Vector2.zero;
    }

    public override void Sense()
    {
        _hungryWolvesSeen = GetHungryWolvesInRadius(3);
        _grassSeen = GetGrassInRadius(2);
    }

    public override void Decide()
    {
        var wasBreeding = state is AnimalState.Breeding;

        isReadyToBreed = (currentHealth > SHEEP_MAX_HEALTH * BREEDING_COEFFICIENT);
        isHungry = (currentHealth < SHEEP_MAX_HEALTH * HUNGER_COEFFICIENT);
        if (occupiedTile == tileToWander)
            tileToWander = WorldGrid.Instance.GetRandomTile();

        if (_hungryWolvesSeen.Count > 0)
        {
            _evadeDirection = GetDirectionToRun();
            state = AnimalState.Evading;
        }

        else if (_grassToEat != null && occupiedTile == _grassToEat.OccupiedTile && _grassToEat.enabled)
            state = AnimalState.Eating;

        else if (isReadyToBreed)
            state = AnimalState.Breeding;

        else if (isHungry)
        {
            _grassToEat = GetGrassToEat();
            state = _grassToEat != null ? AnimalState.Pursuing : AnimalState.Wandering;
        }

        else
            state = AnimalState.Wandering;
        
        if(!wasBreeding)
            UpdateStateColor();
    }

    #region FSM
    private void Update()
    {
        occupiedTile = WorldGrid.Instance.WorldToTilePoint(transform.position);
        currentHealth -= SHEEP_HEALTH_DEPLETION_RATE * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, SHEEP_MAX_HEALTH);
        UpdateHealthColor(SHEEP_MAX_HEALTH);
        ClampPosition();

        if (currentHealth == 0)
            Die();

        switch (state)
        {
            case AnimalState.Evading:
                transform.Translate(_evadeDirection * (SHEEP_RUNNING_SPEED * Time.deltaTime));
                break;

            case AnimalState.Eating:
                Eat(occupiedTile.GrassComponent, SHEEP_EATING_RATE, SHEEP_WALKING_SPEED);
                break;

            case AnimalState.Breeding:
                Breed(SHEEP_MAX_HEALTH);
                Decide();
                break;

            case AnimalState.Pursuing:
                MoveTowards(_grassToEat, SHEEP_WALKING_SPEED);
                break;

            case AnimalState.Wandering:
                MoveTowards(tileToWander, SHEEP_WALKING_SPEED);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion

    private List<Grass> GetGrassInRadius(ushort tileRadius)
    {
        var grassToReturn = new List<Grass>();
        var allGrass = Main.GetEntitiesOfType<Grass>();

        foreach (var grass in allGrass)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - grass.transform.position));

            if (tileRadius * WorldGrid.WORLD_STEP < distance) continue;

            grassToReturn.Add(grass);
        }

        return grassToReturn;
    }

    private Grass GetGrassToEat()
    {
        _grassSeen = GetGrassInRadius(2);
        return _grassSeen.Count == 0 ? null : _grassSeen[Random.Range(0, _grassSeen.Count)];
    }

    private List<Wolf> GetHungryWolvesInRadius(ushort tileRadius)
    {
        var allWolves = Main.GetEntitiesOfType<Wolf>();
        var wolvesNearby = new List<Wolf>();

        foreach (var wolf in allWolves)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - wolf.transform.position));
            if (tileRadius * WorldGrid.WORLD_STEP < distance || !wolf.IsHungry) continue;

            wolvesNearby.Add(wolf);
        }

        return wolvesNearby;
    }

    private Vector2 GetDirectionFromWorldBorders()
    {
        var vectorToReturn = Vector2.zero;

        if(Math.Abs(transform.position.x - WorldGrid.WORLD_BORDER_LEFT) < WorldGrid.WORLD_STEP)
            vectorToReturn += Vector2.right;
        if(Math.Abs(transform.position.x - WorldGrid.WORLD_BORDER_RIGHT) < WorldGrid.WORLD_STEP)
            vectorToReturn += Vector2.left;
        if(Math.Abs(transform.position.y - WorldGrid.WORLD_BORDER_TOP) < WorldGrid.WORLD_STEP)
            vectorToReturn += Vector2.down;
        if(Math.Abs(transform.position.y - WorldGrid.WORLD_BORDER_BOTTOM) < WorldGrid.WORLD_STEP)
            vectorToReturn += Vector2.up;

        return vectorToReturn.normalized;
    }

    ///<returns>A normalized vector that is sum of all wolves' positions and avoidance vectors</returns>
    private Vector2 GetDirectionToRun()
    {
        _hungryWolvesSeen = GetHungryWolvesInRadius(3);
        if(_hungryWolvesSeen.Count == 0)
            return Vector2.zero;

        var vectorToReturn = Vector2.zero;
        foreach (var wolf in _hungryWolvesSeen)
        {
            var vector = (Vector2)(this.transform.position - wolf.transform.position);
            vectorToReturn += vector.normalized;
        }

        vectorToReturn += GetDirectionFromWorldBorders();

        return vectorToReturn.normalized;
    }
}
