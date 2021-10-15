using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    public const int WOLF_MAX_HEALTH = 150;

    private void Awake()
    {
        entityType = EntityType.Animal;
        currentHealth = WOLF_MAX_HEALTH;
        animalType = AnimalType.Wolf;
        speed = 30;
        healthColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
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
