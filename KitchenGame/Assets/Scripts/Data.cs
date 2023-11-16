using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

    public static readonly Dictionary<Ingredients, Vector2Int[]> Cells = new Dictionary<Ingredients, Vector2Int[]>()
    {
        { Ingredients.FLOUR, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1) } },
        { Ingredients.EGGS, new Vector2Int[] { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)} },
        { Ingredients.BUTTER, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 1), new Vector2Int(1, 1) } },
        { Ingredients.CHOC_CHIP, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(1, -1) } },
        { Ingredients.PEANUT_BUTTER, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(2, 0) } },
        { Ingredients.MAPLE_SYRUP, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, -1) } },
        { Ingredients.STRAWBERRY, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1) } },
        { Ingredients.SUGAR, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, -1) } },
        { Ingredients.MILK, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(-1, 1), new Vector2Int(1, 0), new Vector2Int(2, 0) } },
        { Ingredients.BACON, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(0, -2), new Vector2Int(1, 0), new Vector2Int(2, 0) } },
        { Ingredients.BANANA, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, -1), new Vector2Int(1, -1) } },
        { Ingredients.BLUEBERRY, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0) } },
        { Ingredients.WHIPPED_CREAM, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(1, 1) } },
        { Ingredients.ROASTED_PECAN, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0) } },

        { Ingredients.SALSA, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(-1, -2), new Vector2Int(-1, 1) } },
        { Ingredients.CORN, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(1, 0) } },
        { Ingredients.TOMATO, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(1, -1), new Vector2Int(1, 0) } },
        { Ingredients.DORITO, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(-1, 0) } },
        { Ingredients.FRITO, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(-1, 0) } },
        { Ingredients.SHREDDED_CHEESE, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(2, 0) } },
        { Ingredients.SALT, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1) } },
        { Ingredients.BEAN, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, -1), new Vector2Int(1, -1) } },
        { Ingredients.JALAPENO, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 2), new Vector2Int(0, -1), new Vector2Int(-1, -1) } },
        { Ingredients.CHORIZO, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(0, -1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(1, 0), new Vector2Int(2, 0) } },

        { Ingredients.POW_SUGAR, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 1), new Vector2Int(1, 1), new Vector2Int(1, -1) } },
        { Ingredients.WAX, new Vector2Int[] { new Vector2Int(0, -1), new Vector2Int(0, 1) } },
        { Ingredients.ICING, new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(-1, 0),  new Vector2Int(-2, -1), new Vector2Int(1, 0), new Vector2Int(2, -1),} },

        { Ingredients.ENCHANTED, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(-1, 0), new Vector2Int(0, 0),  new Vector2Int(1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1)} },
    };

}