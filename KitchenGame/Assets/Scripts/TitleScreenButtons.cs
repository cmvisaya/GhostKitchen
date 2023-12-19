using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenButtons : MonoBehaviour
{

    public int otherButtonID;
    private TitleScreenManager sm;
    public GameObject border;
    private AudioManager am;

    void Start() {
        sm = GameObject.Find("TitleScreenManager").GetComponent<TitleScreenManager>();
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if(border != null) {
            border.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        sm.onOtherButton = true;
        sm.otherButtonID = otherButtonID;
        if(border != null) { border.SetActive(true); }
        if(sm.buttonPlayable) {
            sm.buttonPlayable = false;
        }
    }

    void OnMouseExit()
    {
        sm.onOtherButton = false;
        sm.otherButtonID = -1;
        if(border != null) { border.SetActive(false); }
        sm.buttonPlayable = true;
    }
}
