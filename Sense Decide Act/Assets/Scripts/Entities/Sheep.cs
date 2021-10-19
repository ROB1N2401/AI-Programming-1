using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sheep : Animal
{
    public enum State
    {
        Evading,
        Eating,
        Breeding,
        Finding,
        Wandering
    }

    public const int SHEEP_MAX_HEALTH = 90;
    public const int SHEEP_EATING_RATE = 30;

    private State _state;
    private Grass _nearestGrass;
    private Vector2 _evadeDirection;

    public State StateProperty => _state;

    private void Awake()
    {
        entityType = EntityType.Sheep;
        currentHealth = SHEEP_MAX_HEALTH * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);
        isHungry = false;
        isReadyToBreed = false;
        speed = 10;
        stateSprite = transform.GetComponent<SpriteRenderer>();
        healthSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _state = State.Wandering;
        _nearestGrass = null;
        _evadeDirection = Vector2.zero;
    }

    private void Start()
    {
        tileToWander = WorldGrid.Instance.GetRandomTile();
    }

    public override void Sense()
    {
        _evadeDirection = GetDirectionToRun(3);
        _nearestGrass = GetNearestGrass(2);
    }

    public override void Decide()
    {
        isReadyToBreed = (currentHealth > SHEEP_MAX_HEALTH * BREEDING_COEFFICIENT);
        isHungry = (currentHealth < SHEEP_MAX_HEALTH * HUNGER_COEFFICIENT);

        if (_evadeDirection != Vector2.zero)
            _state = State.Evading;

        else if (isReadyToBreed)
            _state = State.Breeding;

        else if (_nearestGrass == occupiedTile.GrassComponent)
            _state = State.Eating;

        else if (isHungry && _nearestGrass != null)
            _state = State.Finding;

        else
        {
            tileToWander = WorldGrid.Instance.GetRandomTile();
            _state = State.Wandering;
        }

        UpdateStateColor();
    }

    #region FSM
    // Update is called once per frame
    void Update()
    {
        occupiedTile = WorldGrid.Instance.WorldToTilePoint(transform.position);
        currentHealth -= HEALTH_DEPLETION_RATE * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, SHEEP_MAX_HEALTH);
        UpdateHealthColor(SHEEP_MAX_HEALTH);

        if (currentHealth == 0)
            Die();

        switch (_state)
        {
            case State.Evading:
                transform.Translate(_evadeDirection * (speed * Time.deltaTime));
                break;
            case State.Eating:
                Eat(occupiedTile.GrassComponent, SHEEP_EATING_RATE);
                break;
            case State.Breeding:
                Breed(SHEEP_MAX_HEALTH);
                Decide();
                break;
            case State.Finding:
                MoveTowards(_nearestGrass);
                break;
            case State.Wandering:
                MoveTowards(tileToWander);
                break;
        }
    }
    #endregion

    private Grass GetNearestGrass(ushort tileRadius)
    {
        if (occupiedTile.GrassComponent.enabled)
            return occupiedTile.GrassComponent;

        Grass grassToReturn = null;
        var shortestDistance = 10000.0f;

        foreach (var grass in Main.Instance.GrassCollection)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - grass.Value.transform.position));

            if (distance > shortestDistance || tileRadius * WorldGrid.WORLD_STEP < distance) continue;

            shortestDistance = distance;
            grassToReturn = grass.Value;
        }
        return grassToReturn;
    }

    /// <param name="tileRadius">radius in which a sheep can sense wolves</param>
    /// <returns>a normalized vector that is sum of all wolves' positions</returns>
    private Vector2 GetDirectionToRun(ushort tileRadius)
    {
        var wolvesNearby = new List<Wolf>();

        foreach (var wolf in Main.Instance.WolvesCollection)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - wolf.Value.transform.position));

            if (tileRadius * WorldGrid.WORLD_STEP < distance) continue;

            wolvesNearby.Add(wolf.Value);
        }

        if(wolvesNearby.Count == 0)
            return Vector2.zero;

        var vectorToReturn = Vector3.zero;
        foreach (var wolf in wolvesNearby)
        {
            var vector = this.transform.position - wolf.transform.position;
            vectorToReturn += vector;
        }

        return vectorToReturn.normalized;
    }

    protected override void UpdateStateColor()
    {
        switch (_state)
        {
            case State.Evading:
                stateSprite.color = Color.yellow;
                break;

            case State.Eating:
                stateSprite.color = Color.cyan;
                break;

            case State.Breeding:
                stateSprite.color = ColorLibrary.pink;
                break;

            case State.Finding:
                stateSprite.color = ColorLibrary.orange;
                break;

            case State.Wandering:
                stateSprite.color = Color.white;
                break;
        }
    }
}
