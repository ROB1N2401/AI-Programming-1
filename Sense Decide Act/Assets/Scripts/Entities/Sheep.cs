using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Animal
{
    private void Awake()
    {
        entityType = EntityType.Animal;
        animalType = AnimalType.Sheep;
        speed = 20;
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
