using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableCard : MonoBehaviour
{

    public int cardNum;
    private GameManager gm;

    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnMouseOver()
    {
        gm.currentCardPos = cardNum;
    }
}
