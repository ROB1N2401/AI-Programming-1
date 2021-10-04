using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Support;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Main : MonoSingleton<Main>
{
    [HideInInspector] public List<Entity> Entities;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;

        WorldGrid.Instance.CreateGrid();
        InitializeEntities();
    }

    private void Update()
    {
        float mousePosX = Mathf.Clamp(Input.mousePosition.x, 0, Screen.width);
        float mousePosY = Mathf.Clamp(Input.mousePosition.y, 0, Screen.height);
        var cursorPos = _camera.ScreenToWorldPoint(new Vector3(mousePosX, mousePosY, 0));

        var tile = WorldGrid.Instance.WorldToNodePoint(cursorPos);
        if(tile == null)
            return;

        if(Input.GetMouseButtonDown(0))
            tile.SwitchTileType();
    }

    private void InitializeEntities()
    {
        var starchaser = Instantiate(Resources.Load("Starchaser", typeof(GameObject)), this.gameObject.transform) as GameObject;
        if (!(starchaser is null))
        {
            var entityComponent = starchaser.GetComponent<Entity>();
            entityComponent.SpawnEntity();
            Entities.Add(entityComponent);
        }
    }
}
