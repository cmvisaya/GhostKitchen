using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectButtons : MonoBehaviour
{

    public int otherButtonID; //Scene Code
    private LeveLSelectManager lsm;
    public GameObject border;
    private AudioManager am;

    void Start() {
        lsm = GameObject.Find("Level Select Manager").GetComponent<LeveLSelectManager>();
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if(border != null) {
            border.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        lsm.onOtherButton = true;
        lsm.otherButtonID = otherButtonID;
        if(border != null) { border.SetActive(true); }
        if(lsm.buttonPlayable) {
            lsm.buttonPlayable = false;
        }
    }

    void OnMouseExit()
    {
        lsm.onOtherButton = false;
        lsm.otherButtonID = -1;
        if(border != null) { border.SetActive(false); }
        lsm.buttonPlayable = true;
    }
}
