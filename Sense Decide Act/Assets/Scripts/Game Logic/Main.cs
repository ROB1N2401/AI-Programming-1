using System.Collections.Generic;
using Support;
using UnityEngine;

public class Main : MonoSingleton<Main>
{
    public const float SENSE_UPDATE_PERIOD = 0.25f;
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
            Animal.Instantiate(AnimalType.Sheep);

        Animal.Instantiate(AnimalType.Wolf);
    }
}
