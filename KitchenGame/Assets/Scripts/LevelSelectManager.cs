using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    public Texture2D[] bgs;
    public string[] levelNames;
    public int[] levelIds;
    public RawImage bg;
    public TextMeshProUGUI levelName;
    private int currentMenuItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        bg.texture = bgs[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        bg.texture = bgs[currentMenuItem];
        levelName.text = levelNames[currentMenuItem];
        if(Input.GetKeyDown(KeyCode.Z)) {
            LoadLevel(levelIds[currentMenuItem]);
        } else if (Input.GetButtonDown("Right")) {
            currentMenuItem = (currentMenuItem + 1) % bgs.Length;
        } else if (Input.GetButtonDown("Left")) {
            currentMenuItem = (currentMenuItem + bgs.Length - 1) % bgs.Length;
        }
    }

    void LoadLevel(int levelId) {
        SceneManager.LoadScene(levelId);
    }
}
