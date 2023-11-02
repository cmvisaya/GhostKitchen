using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OtherButtons : MonoBehaviour
{

    public int otherButtonID;
    private GameManager gm;
    public GameObject border;

    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        border.SetActive(false);
    }

    void OnMouseOver()
    {
        if(!gm.inPlacement) {
            gm.DeselectAll();
            gm.onOtherButton = true;
            gm.otherButtonID = otherButtonID;
            border.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        gm.onOtherButton = false;
        border.SetActive(false);
    }
}
