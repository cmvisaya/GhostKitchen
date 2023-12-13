using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectButtons : MonoBehaviour
{

    public int otherButtonID; //Scene Code
    private LeveLSelectManager lsm;
    public GameObject border;

    void Start() {
        lsm = GameObject.Find("Level Select Manager").GetComponent<LeveLSelectManager>();
        if(border != null) {
            border.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        lsm.onOtherButton = true;
        lsm.otherButtonID = otherButtonID;
        if(border != null) { border.SetActive(true); }
    }

    void OnMouseExit()
    {
        lsm.onOtherButton = false;
        lsm.otherButtonID = -1;
        if(border != null) { border.SetActive(false); }
    }
}
