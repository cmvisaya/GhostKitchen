using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int ingredientId;
    public string ingredientName;
    public Texture2D image;
    [HideInInspector] public int arrayIndex;
}
