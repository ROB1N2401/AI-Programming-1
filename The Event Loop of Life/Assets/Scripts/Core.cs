using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject grass = new GameObject("Grass Grid");
        Grid grassGrid = grass.AddComponent<Grid>();
        grassGrid.CreateGrid(grass);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
