
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

    private void Awake()
    {
        _grassComponent = GetComponent<Grass>();
        _grassComponent.enabled = false;
    }

    //Method carries a role of constructor, hence it's positioned above other methods
    public void Initialize(bool hasGrass, Vector3 worldPos, int gridPosX, int gridPosY)
    {
        _worldPos = worldPos; 
        _gridPosX = gridPosX;
        _gridPosY = gridPosY;

        if (hasGrass)
            Grass.Instantiate(this);
        UpdateColor();
    }

    private void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = _grassComponent.enabled ? Color.green : new Color(0.3f, 0.1f, 0.0f);
    }

    /// <summary>
    /// Checks whether the tile is occupied or not
    /// </summary>
    /// <param name="tile">tile to check</param>
    /// <returns>true if tile is occupied by a single entity</returns>
    public static bool CheckIfTileIsOccupied(Tile tile)
    {
        var sheepList = Main.Instance.SheepCollection;
        var wolvesList = Main.Instance.WolvesCollection;

        return (sheepList.Any(t => t.Value.OccupiedTile == tile) 
                ||
                wolvesList.Any(t => t.Value.OccupiedTile == tile));
    }

    public void SwitchTileState()
    {
        _grassComponent.enabled = !_grassComponent.enabled;
        UpdateColor();
    }
}
