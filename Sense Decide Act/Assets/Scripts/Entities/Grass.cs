using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    protected override void Die()
    {
        GetComponent<Tile>().SwitchTileState();
        Main.Instance.GrassCollection.Remove(this.GetInstanceID());
    }

    public static void Instantiate(Tile tile)
    {
        var grass = tile.GetComponent<Grass>();
        if (grass.enabled)
        {
            Debug.LogWarning($"There is grass already on the tile {tile.GridPosX}, {tile.GridPosY}");
            return;
        }
        tile.SwitchTileState();
        Main.Instance.GrassCollection.Add(tile.gameObject.GetInstanceID(), grass);
    }
}
