﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Starchaser : Entity
{
    private const uint STAMINA = 15;
    private const float STEP_PERIOD = 0.25f;

    private uint _currentStamina;
    private bool _hasStar;
    private List<Tile> _path;
    private LineRenderer _lr;
    private EntityType _goingTo;

    private void Start()
    {
        _hasStar = false;
        _currentStamina = STAMINA;
        _path = null;
        _lr = GetComponent<LineRenderer>();
        _goingTo = EntityType.Star;

        UpdateColor();
    }

    #region FSM
    private IEnumerator RunStarchaser()
    {
        while (true)
        {
            switch (_goingTo)
            {
                case EntityType.Star:
                    FindStar();
                    break;
                
                case EntityType.TradingPost:
                    SellStar();
                    break;

                case EntityType.Spaceship:
                    Rest();
                    break;

                default:
                    Debug.LogError("An incorrect EntityType has been set as destination!");
                    break;
            }

            yield return new WaitForSeconds(STEP_PERIOD);
        }
    }

    private void FindStar()
    {
        if(_path == null)
            GetPath("Star");
        else
        {
            if (MoveTowardsTarget())
            {
                GrabStar();
                _goingTo = EntityType.TradingPost;
            }
        }
    }

    private void SellStar()
    {
        if(_path == null)
            GetPath("Trading Post");
        else
        {
            if (MoveTowardsTarget())
            {
                DropStar();
                Main.Instance.Entities["Star"].PlaceEntityOnRandomTile();
                if (_currentStamina != 0)
                    _goingTo = EntityType.Star;
                else 
                    _goingTo = EntityType.Spaceship; 
            }
            else if (_currentStamina == 0)
            {
                DropStar();
                _path = null;
                _goingTo = EntityType.Spaceship;
            }
        }
    }

    private void Rest()
    {
        if(_path == null)
            GetPath("Spaceship");
        else
        {
            if (MoveTowardsTarget())
            {
                RestoreStamina();
                _goingTo = EntityType.Star;
            }
        }
    }
    #endregion

    private void GrabStar()
    {
        _hasStar = true;
    }

    private void DropStar()
    {
        _hasStar = false;
    }

    private void UpdateColor()
    {
        var staminaLeft = (float)_currentStamina / STAMINA;
        transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f - staminaLeft, staminaLeft, 0f);
    }

    private void RestoreStamina()
    {
        _currentStamina = STAMINA;
        UpdateColor();
    }

    private void DrawPath()
    {
        if (_path != null)
        {
            _lr.positionCount = _path.Count;
            for (var i = 0; i < _lr.positionCount; i++)
            {
                _lr.SetPosition(i, _path[i].WorldPos);
            }
        }
        else
        {
            _lr.positionCount = 0;
        }
    }

    private void GetPath(string entityName)
    {
        _path = Pathfinder.FindPath(occupiedTile, Main.Instance.Entities[entityName].OccupiedTile);
        DrawPath();
    }

    /// <summary>
    /// Moves Starchaser one tile forward along the set path. Returns true when Starchaser reaches the target tile
    /// </summary>
    /// <returns></returns>
    private bool MoveTowardsTarget()
    {
        var nextTile = _path[1];
        var delta = (Math.Abs(occupiedTile.GridPosX - nextTile.GridPosX)) + (Math.Abs(occupiedTile.GridPosY - nextTile.GridPosY));
        if (delta == 1)
        {
            if (_hasStar)
            {
                _currentStamina--;
                UpdateColor();
                Main.Instance.Entities["Star"].SetEntitysPosition(nextTile);
            }

            SetEntitysPosition(nextTile);
            _path.RemoveAt(0);
            DrawPath();

            var targetTile = _path[_path.Count - 1];
            if (occupiedTile != targetTile) return false;

            _path = null;
            return true;
        }
        
        Debug.LogError("Attempting to perform an impossible move");

        return false;
    }

    public void LaunchStarchaser()
    {
        StartCoroutine(RunStarchaser());
    }
}
