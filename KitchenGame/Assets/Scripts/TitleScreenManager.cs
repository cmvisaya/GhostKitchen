using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public AudioManager am;
    public RawImage bg;
    public Texture2D[] screens;

    public GameObject buttons;
    public int otherButtonID = -1;
    public bool onOtherButton = false;

    public int lvlSelSceneID;

    // Start is called before the first frame update
    void Start()
    {
        bg.texture = screens[0];
        StartCoroutine(Startup());
    }

    IEnumerator Startup() {
        am.StopBGM();
        am.Play(2, 1f);
        yield return new WaitForSeconds(0.4f);
        bg.texture = screens[1];
        yield return new WaitForSeconds(1f);
        buttons.SetActive(true);
        am.PlayBGM(0);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) {
            if(onOtherButton) {
                switch(otherButtonID) {
                    case 0:
                        Application.Quit();
                        break;
                    case 1:
                        SceneManager.LoadScene(lvlSelSceneID);
                        break;
                }
            }
        }
    }
}
