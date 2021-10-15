using UnityEngine;

public class Grass : Entity
{
    public enum State
    {
        Growing, 
        Withering,
        Spreading,
        Trampled
    }

    private const float MIN_STARTING_HEALTH_COEFFICIENT = 0.1f;
    private const float MAX_STARTING_HEALTH_COEFFICIENT = 0.2f;
    private const float MATURITY_TIME_SPAN = 5.0f; //in seconds
    private const int SPREADING_CHANCE = 15; //in percents, per frame
    private const int HEALTH_DEPLETION_RATE = 3;
    public const int GRASS_MAX_HEALTH = 30;

    private SpriteRenderer _spriteRenderer;
    private State _state;
    private float _maturityTimeElapsed;
    private bool _isTrampled;
    private bool _isEaten;
    private bool _hasReachedMaturity;


    private void Awake()
    {
        occupiedTile = GetComponent<Tile>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = 1.0f;
        entityType = EntityType.Grass;
        _state = State.Growing;
        _maturityTimeElapsed = 0.0f;
    }

    private void OnEnable()
    {
        currentHealth = GRASS_MAX_HEALTH * Random.Range(MIN_STARTING_HEALTH_COEFFICIENT, MAX_STARTING_HEALTH_COEFFICIENT);
        _state = State.Growing;
        _maturityTimeElapsed = 0.0f;
    }

    private void OnDisable()
    {
        occupiedTile.UpdateColor();
    }

    public override void Sense()
    {
        _hasReachedMaturity = (_maturityTimeElapsed > 0.0f) || (currentHealth >= GRASS_MAX_HEALTH && _maturityTimeElapsed == 0.0f);
        _isTrampled = Tile.CheckIfTileIsOccupiedByAnimal(this.OccupiedTile);
        _isEaten = CheckIfBeingEaten();
    }

    public override void Decide()
    {
        if (!_isTrampled && !_hasReachedMaturity)
        {
            _state = State.Growing;
            return;
        }

        if (_maturityTimeElapsed > MATURITY_TIME_SPAN)
        {
            _state = State.Withering;
            return;
        }

        if (_hasReachedMaturity && !_isEaten)
        {
            _state = State.Spreading;
            return;
        }

        if (_isTrampled)
        {
            _state = State.Trampled;
        }
    }

    #region FSM
    private void Update()
    {
        UpdateHealthColor(GRASS_MAX_HEALTH);

        if (currentHealth == 0)
            Die();

        switch (_state)
        {
            case State.Growing:
                Grow();
                break;

            case State.Spreading:
                Spread();
                break;

            case State.Withering:
                Wither();
                break;

            case State.Trampled:
                break;
        }
    }
    
    private void Grow()
    {
        currentHealth += HEALTH_DEPLETION_RATE * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, GRASS_MAX_HEALTH);
    }

    private void Wither()
    {
        currentHealth -= HEALTH_DEPLETION_RATE * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, GRASS_MAX_HEALTH);
    }

    private void Spread()
    {
        _maturityTimeElapsed += Time.deltaTime;
        if (!(Random.Range(0.0f, 1.0f) * 100 <= SPREADING_CHANCE / 100.0f)) return;

        var tile = GetNearestFreeTile();
        if(tile != null)
            Instantiate(tile);
    }
    #endregion

    private bool CheckIfBeingEaten()
    {
        var sheepList = Main.Instance.SheepCollection;

        foreach (var sheep in sheepList)
        {
            if (sheep.Value.OccupiedTile != this.OccupiedTile) continue;

            if (sheep.Value.StateProperty == Sheep.State.Eating)
                return true;
        }

        return false;
    }

    protected override void UpdateHealthColor(int maxHealth)
    {
        var healthLeft = currentHealth / maxHealth;
        var r = Mathf.Lerp(0.0f, 0.6f, 1 - healthLeft);
        var g = Mathf.Lerp(0.4f, 1.0f, healthLeft);
        _spriteRenderer.color = new Color(r, g, 0f);
    }

    protected override void Die()
    {
        occupiedTile.SetGrassComponentState(false);
        Main.Instance.GrassCollection.Remove(GetInstanceID());
    }

    public static void Instantiate(Tile tile)
    {
        var grass = tile.GetComponent<Grass>();

        //Debug.Log($"{tile.name} has ID {tile.GetInstanceID()}, {tile.gameObject.GetInstanceID()}");

        tile.SetGrassComponentState(true);
        if(!Main.Instance.GrassCollection.ContainsKey(tile.GetInstanceID()))
            Main.Instance.GrassCollection.Add(tile.GetInstanceID(), grass);
    }
}
