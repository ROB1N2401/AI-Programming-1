using UnityEngine;

public class Grass : Entity
{
    private enum State
    {
        Growing, 
        Withering,
        Spreading,
        Trampled
    }

    public const int GRASS_MAX_HEALTH = 30;

    private const float MATURITY_TIME_SPAN = 5.0f; //in seconds
    private const int SPREADING_CHANCE = 30; //chance to spawn a grass tile each second
    private const int GRASS_HEALTH_DEPLETION_RATE = GRASS_MAX_HEALTH / 10;


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
        _isTrampled = Tile.CheckIfTileIsOccupiedByAnimal(this.OccupiedTile);
        _isEaten = CheckIfBeingEaten();
    }

    public override void Decide()
    {
        _hasReachedMaturity = (_maturityTimeElapsed > 0.0f) || (currentHealth >= GRASS_MAX_HEALTH && _maturityTimeElapsed == 0.0f);

        if (!_isTrampled && !_hasReachedMaturity)
            _state = State.Growing;

        else if (_maturityTimeElapsed > MATURITY_TIME_SPAN)
            _state = State.Withering;

        else if (_hasReachedMaturity && !_isEaten) 
            _state = State.Spreading;

        else if (_isTrampled)
            _state = State.Trampled;
    }

    #region FSM
    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, GRASS_MAX_HEALTH);
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
        currentHealth += GRASS_HEALTH_DEPLETION_RATE * Time.deltaTime;
    }

    private void Wither()
    {
        currentHealth -= GRASS_HEALTH_DEPLETION_RATE * Time.deltaTime;
    }

    private void Spread()
    {
        _maturityTimeElapsed += Time.deltaTime;
        if (!(Random.Range(0.0f, 1.0f) * 100 <= SPREADING_CHANCE * Time.deltaTime)) return;

        var tile = GetNearestFreeTile(this);
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

            if (sheep.Value.State == AnimalState.Eating)
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
        if(!Main.Instance.GrassCollection.ContainsKey(grass.GetInstanceID()))
            Main.Instance.GrassCollection.Add(grass.GetInstanceID(), grass);
    }
}
