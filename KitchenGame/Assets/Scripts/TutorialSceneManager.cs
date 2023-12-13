using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceneManager : MonoBehaviour
{
    public GameObject tutorialWindow;
    public RawImage bg;
    public Texture2D[] textures;
    int currentItem = 0;
    public bool inTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        //tutorialWindow.SetActive(false);
        currentItem = 0;
        bg.texture = textures[currentItem];
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialWindow.activeSelf && (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1"))) {
            currentItem++;
            if(currentItem < textures.Length) { bg.texture = textures[currentItem]; }
            else { tutorialWindow.SetActive(false); inTutorial = false; }
        }
    }

    public void BeginTutorial() {
        tutorialWindow.SetActive(true);
        inTutorial = true;
    }
}
