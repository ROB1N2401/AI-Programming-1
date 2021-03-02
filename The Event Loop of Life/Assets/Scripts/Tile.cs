using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tile
{
    public int GridPosX;
    public int GridPosY;
    public Vector3 WorldPos;
    public SpriteRenderer renderer = null;
    public GameObject parent;

    public Tile(Vector3 worldPos_in, GameObject parent_in, int gridPosX_in, int gridPosY_in)
    {
        WorldPos = worldPos_in;
        GridPosX = gridPosX_in;
        GridPosY = gridPosY_in;

        Transform parentTransform = parent_in.GetComponent<Transform>();

        GameObject gameObject = new GameObject("Tile");
        gameObject.transform.SetParent(parentTransform, true);
        gameObject.transform.localScale = new Vector2(0.3f, 0.3f);
        gameObject.transform.position = WorldPos;

        renderer = gameObject.AddComponent<SpriteRenderer>();
        Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Square Shape.png", typeof(Sprite));
        renderer.sprite = sprite;
    }
}
