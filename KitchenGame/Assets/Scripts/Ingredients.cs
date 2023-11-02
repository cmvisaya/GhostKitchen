using UnityEngine;
using UnityEngine.Tilemaps;

public enum Ingredients
{
    FLOUR,
    EGGS,
    BUTTER,
    CHOC_CHIP,
    PEANUT_BUTTER,
    MAPLE_SYRUP,
    STRAWBERRY,
    SUGAR,
    MILK,
    BACON,
    BANANA,
    BLUEBERRY,
    WHIPPED_CREAM,
    ROASTED_PECAN,
}

public enum Flavors
{
    SWEET,
    RICH,
    PLAIN,
    TART,
    SALTY,
    SPICY
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