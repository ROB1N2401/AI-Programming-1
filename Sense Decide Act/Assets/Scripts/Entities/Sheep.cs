using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal
{
   public enum State
    {
        EvadingWolves,
        Eating,
        Breeding,
        SearchingForGrass,
        Wandering
    }

    public const int SHEEP_MAX_HEALTH = 90;

    private State _state;

    public State StateProperty => _state;

    private void Awake()
    {
        entityType = EntityType.Animal;
        currentHealth = SHEEP_MAX_HEALTH;
        animalType = AnimalType.Sheep;
        speed = 20;
        healthColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        _state = State.Wandering;
    }

    // Start is called before the first frame update
    void Start()
    {
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
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
}
