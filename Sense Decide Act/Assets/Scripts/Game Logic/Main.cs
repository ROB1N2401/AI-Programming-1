using System.Collections;
using System.Collections.Generic;
using Support;
using UnityEngine;
// ReSharper disable IteratorNeverReturns

public class Main : MonoSingleton<Main>
{
    public const float SENSE_UPDATE_PERIOD = 0.1f;
    public const float DECIDE_UPDATE_PERIOD = 0.4f;

    public Dictionary<int, Entity> EntityCollection;

    private void Start()
    {
        EntityCollection = new Dictionary<int, Entity>();

        WorldGrid.Instance.CreateGrid();
        InitializeAnimals();

        StartCoroutine(SenseRoutine());
        StartCoroutine(DecisionRoutine());
    }

    private IEnumerator SenseRoutine()
    {
        while (true)
        {
            foreach (var e in EntityCollection)
                e.Value.Sense();
            yield return new WaitForSeconds(SENSE_UPDATE_PERIOD);
        }
    }

    private IEnumerator DecisionRoutine()
    {
        while (true)
        {
            foreach (var e in EntityCollection)
                e.Value.Decide();
            yield return new WaitForSeconds(DECIDE_UPDATE_PERIOD);
        }
    }

    private static void InitializeAnimals()
    {
        for (var i = 0; i < 10; i++)
            Instantiate(EntityType.Sheep);

        Instantiate(EntityType.Wolf);
    }

    public static List<T> GetEntitiesOfType<T>()
    {
        var entites = new List<T>();

        foreach (var entity in Instance.EntityCollection)
        {
            if(entity.Value is T t)
                entites.Add(t);
        }

        return entites;
    }


    public static Entity Instantiate(EntityType entityTypeIn)
    {
        var entityName = entityTypeIn.ToString();
        if (!(Instantiate(Resources.Load(entityName, typeof(GameObject))) is GameObject go))
        {
            Debug.LogError($"Failed to instantiate animal: {entityName}");
            return null;
        }

        go.name = entityTypeIn + " ";
        var entityComponent = go.GetComponent<Entity>();
        Instance.EntityCollection.Add(go.GetInstanceID(), entityComponent);

        if (!(entityComponent is Animal animalComponent)) 
            return entityComponent;

        animalComponent.PlaceAnimalOnRandomTile();

        return animalComponent;
    }
}
