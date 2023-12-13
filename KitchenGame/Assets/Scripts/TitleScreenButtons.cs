using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenButtons : MonoBehaviour
{

    public int otherButtonID;
    private TitleScreenManager sm;
    public GameObject border;

    void Start() {
        sm = GameObject.Find("TitleScreenManager").GetComponent<TitleScreenManager>();
        if(border != null) {
            border.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        sm.onOtherButton = true;
        sm.otherButtonID = otherButtonID;
        if(border != null) { border.SetActive(true); }
    }

    void OnMouseExit()
    {
        sm.onOtherButton = false;
        sm.otherButtonID = -1;
        if(border != null) { border.SetActive(false); }
    }
}
