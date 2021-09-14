using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int GCost;
    [HideInInspector] public int HCost;
    public int GridPosX;
    public int GridPosY;
    public bool Walkable;
    public Vector3 WorldPos;
    [HideInInspector] public Tile Parent;

    public void Initialize(bool walkableIn, Vector3 worldPosIn, int gridPosXIn, int gridPosYIn)
    {
        Walkable = walkableIn;
        WorldPos = worldPosIn;
        GridPosX = gridPosXIn;
        GridPosY = gridPosYIn;

        if (!Walkable)
            GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f);
    }

    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }

    private void Update()
    {

    }
}
