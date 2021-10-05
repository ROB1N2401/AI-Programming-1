using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Starchaser : Entity
{
    private List<Tile> _path;
    private LineRenderer _lr;

    public List<Tile> Path { get => _path; set => _path = value; }

    // Start is called before the first frame update
    private void Start()
    {
        _path = null;
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DrawPath();
    }

    private void DrawPath()
    {
        if (_path != null)
        {
            _lr.positionCount = _path.Count;
            for (var i = 0; i < _lr.positionCount; i++)
            {
                _lr.SetPosition(i, _path[i].worldPos);
            }
        }
        else
        {
            _lr.positionCount = 0;
        }
    }
}
