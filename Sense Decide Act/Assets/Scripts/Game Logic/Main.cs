using System.Collections.Generic;
using Support;
using UnityEngine;

public class Main : MonoSingleton<Main>
{
    public const float SENSE_UPDATE_PERIOD = 0.1f;
    public const float DECIDE_UPDATE_PERIOD = 0.4f;

    public Dictionary<int, Grass> GrassCollection;
    public Dictionary<int, Sheep> SheepCollection;
    public Dictionary<int, Wolf> WolvesCollection;

    private float _senseTimeElapsed;
    private float _decideTimeElapsed;

    private void Start()
    {
        GrassCollection = new Dictionary<int, Grass>();
        SheepCollection = new Dictionary<int, Sheep>();
        WolvesCollection = new Dictionary<int, Wolf>();
        _senseTimeElapsed = 0.0f;
        _decideTimeElapsed = 0.0f;

        WorldGrid.Instance.CreateGrid();
        InitializeAnimals();
    }

    private void Update()
    {
        if (_senseTimeElapsed >= SENSE_UPDATE_PERIOD)
        {
            _senseTimeElapsed = 0.0f;

            foreach (var e in GrassCollection)
                e.Value.Sense();
            
            foreach (var e in SheepCollection)
                e.Value.Sense();

            foreach (var e in WolvesCollection)
                e.Value.Sense();
        }

        if (_decideTimeElapsed >= DECIDE_UPDATE_PERIOD)
        {
            _decideTimeElapsed = 0.0f;

            foreach (var e in GrassCollection)
                e.Value.Decide();
            
            foreach (var e in SheepCollection)
                e.Value.Decide();

            foreach (var e in WolvesCollection)
                e.Value.Decide();
        }

        _senseTimeElapsed += Time.deltaTime;
        _decideTimeElapsed += Time.deltaTime;
    }

    private static void InitializeAnimals()
    {
        for (var i = 0; i < 10; i++)
            Instantiate(EntityType.Sheep);

        Instantiate(EntityType.Wolf);
    }

    public static Animal Instantiate(EntityType entityTypeIn)
    {
        if (!(Instantiate(Resources.Load(entityTypeIn.ToString(), typeof(GameObject))) is GameObject go))
        {
            Debug.LogError($"Failed to instantiate animal: {entityTypeIn.ToString()}");
            return null;
        }

        go.name = entityTypeIn + " ";
        var animalComponent = go.GetComponent<Animal>();
        switch (entityTypeIn)
        {
            case EntityType.Grass:
                Debug.LogError("This method should not have non-animal entity passed in");
                return null;
            case EntityType.Sheep:
                Instance.SheepCollection.Add(animalComponent.GetInstanceID(), animalComponent.GetComponent<Sheep>());
                go.name += Instance.SheepCollection.Count;
                break;
            case EntityType.Wolf:
                Instance.WolvesCollection.Add(animalComponent.GetInstanceID(), animalComponent.GetComponent<Wolf>());
                go.name += Instance.WolvesCollection.Count;
                break;
        }

        animalComponent.PlaceAnimalOnRandomTile();
        return animalComponent;
    }
}
