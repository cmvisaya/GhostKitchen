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

    SALSA,
    CORN,
    TOMATO,
    DORITO,
    FRITO,
    SHREDDED_CHEESE,
    SALT,
    BEAN,
    CHORIZO,
    JALAPENO,

    POW_SUGAR,
    ICING,
    WAX,

    ENCHANTED,

    CREAM,
    CREAM_CHEESE,
    WHOLE_MILK,
    CINNAMON,
    CHILI_FLAKE,

    SHREDDED_COCONUT,
    LIME_ZEST,

    CARAMEL,
    SOUR_CREAM,
    CRISPY_ONION,

    COCOA_BEAN,

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