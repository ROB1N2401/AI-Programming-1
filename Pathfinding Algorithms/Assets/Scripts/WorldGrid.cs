using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    private Tile[,] tileStorage;

    public void CreateGrid()
    {
        int length = 10;
        int width = 10;
        tileStorage = new Tile[length, width];

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Vector3 pos = new Vector3(i * 5, -j * 5);
                GameObject go = Instantiate(Resources.Load("Tile", typeof(GameObject)), this.gameObject.transform) as GameObject;
                go.transform.position = pos;
                Tile tile = go.GetComponent<Tile>();
                tile.Initialize((Random.Range(0f, 1f) >= 0.5f), pos, i, j);
                tileStorage[i, j] = tile;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
