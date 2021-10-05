using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;


public enum EntityType
{
    Starchaser,
    Star,
    TradingPost,
    Spaceship
}

public class Entity : MonoBehaviour
{
    private Tile _gridPos;
    private EntityType _entityType;

    public Tile GridPos { get => _gridPos; set => _gridPos = value; }
    public EntityType Type => _entityType;

    /// <summary>
    /// Sets entity's position to selected tile, both on grid and in-world. Doesn't consider tie's type, so be careful
    /// </summary>
    /// <param name="tile"></param>
    private void SetEntitysPosition(Tile tile)
    {
        GridPos = tile;
        this.transform.position = GridPos.transform.position;
    }

    private void SpawnEntity(EntityType entityType)
    {
        var x = WorldGrid.Instance.GridSizeX - 1;
        var y = WorldGrid.Instance.GridSizeY - 1;
        _entityType = entityType;

        while (true)
        {
            var i = Random.Range(0, x);
            var j = Random.Range(0, y);

            var tile = WorldGrid.Instance.TileStorage[i, j];
            if (tile.walkable)
            {
                SetEntitysPosition(tile);
                break;
            }
        }
    }

    public static void Instantiate(string resourcePrefabName, EntityType entityType)
    {
        if (Instantiate(Resources.Load(resourcePrefabName, typeof(GameObject))) is GameObject go)
        {
            var entityComponent = go.GetComponent<Entity>();
            entityComponent.SpawnEntity(entityType);
            Main.Instance.Entities.Add(resourcePrefabName, entityComponent);
        }
    }
}
