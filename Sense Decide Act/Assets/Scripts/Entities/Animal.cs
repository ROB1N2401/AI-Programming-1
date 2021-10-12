using UnityEngine;

public enum AnimalType
{
    Sheep,
    Wolf
}

public abstract class Animal : Entity
{
    protected AnimalType animalType;
    protected int speed;

    private void SetAnimalsPosition(Tile tile)
    {
        occupiedTile = tile;
        this.transform.position = occupiedTile.transform.position;
    }

    private void PlaceAnimalOnRandomTile()
    {
        var x = WorldGrid.Instance.GridSizeX - 1;
        var y = WorldGrid.Instance.GridSizeY - 1;

        while (true)
        {
            var i = Random.Range(0, x);
            var j = Random.Range(0, y);

            var tile = WorldGrid.Instance.TileStorage[i, j];

            if (Tile.CheckIfTileIsOccupied(tile)) continue;

            SetAnimalsPosition(tile);
            break;
        }
    }

    protected void MoveTowards(Entity entity)
    {
        Vector3.MoveTowards(this.transform.position, entity.OccupiedTile.WorldPos, speed * Time.deltaTime);
    }

    //TODO: finish method
    protected void Breed(int maxHealth)
    {
        currentHealth -= (maxHealth / 2.0f);
        Instantiate(animalType);
    }

    protected override void Die()
    {
        switch (animalType)
        {
            case AnimalType.Sheep:
                Main.Instance.SheepCollection.Remove(transform.GetInstanceID());
                break;
            case AnimalType.Wolf:
                Main.Instance.WolvesCollection.Remove(transform.GetInstanceID());
                break;
        }
        Destroy(this.gameObject);
    }

    public static void Instantiate(AnimalType animalType)
    {
        if (!(Instantiate(Resources.Load(animalType.ToString(), typeof(GameObject))) is GameObject go))
        {
            Debug.LogError($"Failed to instantiate animal: {animalType.ToString()}");
            return;
        }

        var animalComponent = go.GetComponent<Animal>();
        switch (animalType)
        {
            case AnimalType.Sheep:
                Main.Instance.SheepCollection.Add(animalComponent.gameObject.GetInstanceID(), animalComponent.GetComponent<Sheep>());
                break;
            case AnimalType.Wolf:
                Main.Instance.WolvesCollection.Add(animalComponent.gameObject.GetInstanceID(), animalComponent.GetComponent<Wolf>());
                break;
        }

        animalComponent.PlaceAnimalOnRandomTile();
    }
}
