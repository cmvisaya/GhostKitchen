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
    public bool buttonPlayable = true;

    void Start() {
        tsm = GameObject.Find("TutorialManager").GetComponent<TutorialSceneManager>();
    }
    void Update() {
        if((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) && !tsm.inTutorial) {
            if(otherButtonID == -500) {
                            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(5, 0.3f);

                tsm.BeginTutorial();
            }
            else if(onOtherButton) {
                            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play(5, 0.3f);

                SceneManager.LoadScene(otherButtonID);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }
}
