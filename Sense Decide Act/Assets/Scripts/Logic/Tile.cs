using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Grass))]
public class Tile : MonoBehaviour
{
    private int _gridPosX;
    private int _gridPosY;
    private Vector3 _worldPos;
    private Grass _grassComponent;

    public int GridPosX => _gridPosX;
    public int GridPosY => _gridPosY;
    public Vector3 WorldPos => _worldPos;
    public Grass GrassComponent => _grassComponent;

    private void Awake()
    {
        _grassComponent = GetComponent<Grass>();
        _grassComponent.enabled = false;
    }

    public void Initialize(bool hasGrass, Vector3 worldPos, int gridPosX, int gridPosY)
    {
        _worldPos = worldPos;
        _gridPosX = gridPosX;
        _gridPosY = gridPosY;

        if (hasGrass)
            Grass.Instantiate(this);
        UpdateColor();
    }

    public static bool CheckIfTileIsOccupiedByAnimal(Tile tile)
    {
        var sheepList = Main.GetEntitiesOfType<Sheep>();
        var wolvesList = Main.GetEntitiesOfType<Wolf>();

        return (sheepList.Any(t => t.OccupiedTile == tile) 
                ||
                wolvesList.Any(t => t.OccupiedTile == tile));
    }

    public static bool CheckIfTileHasGrass(Tile tile)
    {
        return tile._grassComponent.enabled;
    }

    public void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = _grassComponent.enabled ? Color.green : ColorLibrary.brown;
    }

    public void SetGrassComponentState(bool value)
    {
        _grassComponent.enabled = value;
    }
}
