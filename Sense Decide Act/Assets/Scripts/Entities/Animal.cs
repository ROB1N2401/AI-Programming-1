using System.Linq;
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
    protected Color healthColor;

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

            if (Tile.CheckIfTileIsOccupiedByAnimal(tile)) continue;

            SetAnimalsPosition(tile);
            break;
        }
    }

    protected void MoveTowards(Entity entity)
    {
        Vector3.MoveTowards(this.transform.position, entity.OccupiedTile.WorldPos, speed * Time.deltaTime);
    }

    protected override void UpdateHealthColor(int maxHealth)
    {
        var healthLeft = currentHealth / maxHealth;
        healthColor = new Color(1f - healthLeft, healthLeft, 0f);
    }

    protected void Breed(int maxHealth)
    {
        currentHealth -= (maxHealth / 2.0f);
        var newAnimal = Instantiate(animalType);
        newAnimal.SetAnimalsPosition(GetNearestFreeTile());
        newAnimal.currentHealth = maxHealth / 2.0f;
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

    public static Animal Instantiate(AnimalType animalTypeIn)
    {
        if (!(Instantiate(Resources.Load(animalTypeIn.ToString(), typeof(GameObject))) is GameObject go))
        {
            Debug.LogError($"Failed to instantiate animal: {animalTypeIn.ToString()}");
            return null;
        }

        var animalComponent = go.GetComponent<Animal>();
        switch (animalTypeIn)
        {
            case AnimalType.Sheep:
                Main.Instance.SheepCollection.Add(animalComponent.gameObject.GetInstanceID(), animalComponent.GetComponent<Sheep>());
                break;
            case AnimalType.Wolf:
                Main.Instance.WolvesCollection.Add(animalComponent.gameObject.GetInstanceID(), animalComponent.GetComponent<Wolf>());
                break;
        }

        animalComponent.PlaceAnimalOnRandomTile();
        return animalComponent;
    }
}
