using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OtherButtons : MonoBehaviour
{

    public int otherButtonID;
    public int otherButtonCode;
    private GameManager gm;
    public GameObject border;

    void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(border != null) {
            border.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        if(!gm.inPlacement) {
            gm.DeselectAll();
            gm.onOtherButton = true;
            gm.otherButtonID = otherButtonID;
            gm.otherButtonCode = otherButtonCode;
            if(border != null) { border.SetActive(true); }
        }
    }

    void OnMouseExit()
    {
        //gm.onOtherButton = false;
        gm.otherButtonID = -1;
        if(border != null) {
            border.SetActive(false);
        }
    }
}
