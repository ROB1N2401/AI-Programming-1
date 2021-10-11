using System.Collections.Generic;
using Support;
using UnityEngine;

public class Main : MonoSingleton<Main>
{
    public Dictionary<int, Grass> GrassTiles;

    private void Start()
    {
        GrassTiles = new Dictionary<int, Grass>();

        WorldGrid.Instance.CreateGrid();
        InitializeEntities();
    }

    private void Update()
    {

    }

    private void InitializeEntities()
    {
 
    }
}
