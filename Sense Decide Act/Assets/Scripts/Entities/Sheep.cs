using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sheep : Animal
{
    public const int SHEEP_MAX_HEALTH = 90;

    private const int SHEEP_EATING_RATE = Grass.GRASS_MAX_HEALTH / 1;
    private const int SHEEP_WALKING_SPEED = 10;
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
        

        UpdateStateColor();
    }

    #region FSM
    void Update()
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
        }
    }
    #endregion

    private List<Grass> GetGrassInRadius(ushort tileRadius)
    {
        var grassToReturn = new List<Grass>();

        foreach (var grass in Main.Instance.GrassCollection)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - grass.Value.transform.position));

            if (tileRadius * WorldGrid.WORLD_STEP < distance) continue;

            grassToReturn.Add(grass.Value);
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
        var wolvesNearby = new List<Wolf>();

        foreach (var wolf in Main.Instance.WolvesCollection)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - wolf.Value.transform.position));
            if (tileRadius * WorldGrid.WORLD_STEP < distance || !wolf.Value.Ishungry) continue;

            wolvesNearby.Add(wolf.Value);
        }

        return wolvesNearby;
    }

    /// <returns>a normalized vector that is sum of all wolves' positions</returns>
    private Vector2 GetDirectionToRun()
    {
        _hungryWolvesSeen = GetHungryWolvesInRadius(3);
        if(_hungryWolvesSeen.Count == 0)
            return Vector2.zero;

        var vectorToReturn = Vector3.zero;
        foreach (var wolf in _hungryWolvesSeen)
        {
            var vector = this.transform.position - wolf.transform.position;
            vectorToReturn += vector;
        }

        return vectorToReturn.normalized;
    }
}
