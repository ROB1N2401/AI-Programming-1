using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    private void Awake()
    {
        entityType = EntityType.Animal;
        animalType = AnimalType.Wolf;
        speed = 30;
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
