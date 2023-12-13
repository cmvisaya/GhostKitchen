using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeveLSelectManager : MonoBehaviour
{
    public int otherButtonID = -1;
    public bool onOtherButton = false;
    TutorialSceneManager tsm;

    void Start() {
        tsm = GameObject.Find("TutorialManager").GetComponent<TutorialSceneManager>();
    }
    void Update() {
        if((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) && !tsm.inTutorial) {
            if(otherButtonID == -500) {
                tsm.BeginTutorial();
            }
            else if(onOtherButton) {
                SceneManager.LoadScene(otherButtonID);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }
}
