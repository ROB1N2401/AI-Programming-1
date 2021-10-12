using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Entity
{
    private void Awake()
    {
        entityType = EntityType.Grass;
    }

    private void Start()
    {
        //Debug.Log("Start called");
    }

    private void Update()
    {
        //Debug.Log("Update called");
    }

    public override void Sense()
    {
        //Debug.Log("Sense called");
    }

    public override void Decide()
    {
        //Debug.Log("Decide called");
    }
}
