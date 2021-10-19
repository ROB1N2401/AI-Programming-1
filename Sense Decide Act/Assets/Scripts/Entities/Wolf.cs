using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    public enum State
    {
        Eating,
        Breeding,
        Pursuing,
        Wandering
    }

    public const int WOLF_MAX_HEALTH = 150;
    public const int WOLF_EATING_RATE = 50;

    private void Awake()
    {
        entityType = EntityType.Wolf;
        currentHealth = WOLF_MAX_HEALTH * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);
        isHungry = false;
        isReadyToBreed = false;
        speed = 20;
        tileToWander = null;
        stateSprite = transform.GetComponent<SpriteRenderer>();
        healthSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //throw new System.NotImplementedException();
    }

    public override void Sense()
    {
        //throw new System.NotImplementedException();
    }

    public override void Decide()
    {
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        //occupiedTile = WorldGrid.Instance.WorldToTilePoint(transform.position);
        //currentHealth -= HEALTH_DEPLETION_RATE * Time.deltaTime;
    }

    private Sheep GetNearestSheep(ushort tileRadius)
    {
        Sheep sheepToReturn = null;
        var shortestDistance = 10000.0f;

        foreach (var sheep in Main.Instance.SheepCollection)
        {
            var distance = Mathf.Abs(Vector3.Magnitude(this.transform.position - sheep.Value.transform.position));

            if (distance > shortestDistance || tileRadius * WorldGrid.WORLD_STEP < distance) continue;

            shortestDistance = distance;
            sheepToReturn = sheep.Value;
        }
        return sheepToReturn;
    }

    protected override void UpdateStateColor()
    {
        throw new System.NotImplementedException();
    }
}
