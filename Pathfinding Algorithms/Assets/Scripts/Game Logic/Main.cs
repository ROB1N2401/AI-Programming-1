﻿using System.Collections.Generic;
using Support;
using UnityEngine;

public class Main : MonoSingleton<Main>
{
    public Dictionary<string, Entity> Entities;

    private bool _simulationIsRunning;
    private Camera _camera;

    private void Start()
    {
        _simulationIsRunning = false;
        Entities = new Dictionary<string, Entity>();
        _camera = Camera.main;

        WorldGrid.Instance.CreateGrid();
        InitializeEntities();
    }

    private void Update()
    {
        var mousePosX = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
        var mousePosY = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);
        var cursorPos = _camera.ScreenToWorldPoint(new Vector3(mousePosX, mousePosY, 0));

        var tile = WorldGrid.Instance.WorldToNodePoint(cursorPos);
        if (tile != null && !_simulationIsRunning)
            HandleInput(tile);
    }

    private static void InitializeEntities()
    {
        Entity.Instantiate("Starchaser", EntityType.Starchaser);
        Entity.Instantiate("Spaceship", EntityType.Spaceship);
        Entity.Instantiate("Star", EntityType.Star);
        Entity.Instantiate("Trading Post", EntityType.TradingPost);
    }

    private void HandleInput(Tile tile)
    {
        if (Input.GetMouseButtonDown(0))
            tile.SwitchTileType();

        if (Tile.CheckIfTileIsOccupied(tile))
            return;

        if (Input.GetKeyDown(KeyCode.Q))
            Entities["Starchaser"].SetEntitysPosition(tile);
        if (Input.GetKeyDown(KeyCode.W))
            Entities["Star"].SetEntitysPosition(tile);
        if (Input.GetKeyDown(KeyCode.E))
            Entities["Trading Post"].SetEntitysPosition(tile);
        if (Input.GetKeyDown(KeyCode.R))
            Entities["Spaceship"].SetEntitysPosition(tile);
    }

    public void RunSimulation()
    {
        if (_simulationIsRunning)
            return;

        _simulationIsRunning = true;
        Entities["Starchaser"].GetComponent<Starchaser>().LaunchStarchaser();
    }

    public void StopSimulation() => _simulationIsRunning = false;

}
