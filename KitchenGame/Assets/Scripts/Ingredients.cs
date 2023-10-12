using UnityEngine;
using UnityEngine.Tilemaps;

public enum Ingredients
{
    SQUARE,
    CHOC_CHIP,
    SUGAR,
    BERRY,
    BACON,
}

public enum Flavors
{
    SWEET,
    SAVORY,
    PLAIN,
    SPICY,
    SALTY,
}

[System.Serializable]
public struct IngredientData {
    public Ingredients ingredient;
    public Flavors flavor;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }

    public void Initialize() {
        this.cells = Data.Cells[this.ingredient];
    }
}