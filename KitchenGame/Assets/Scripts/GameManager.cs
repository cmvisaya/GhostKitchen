using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public GameObject UI;
    public Card[] ingredientCards;
    public GameObject[] selectionBorders;
    public Card[] currentCards;
    [HideInInspector] public int currentCardPos = 0;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI turnText;
    public RawImage[] cardImages;
    private Board board;
    public bool zCooling = false;
    public bool inPlacement = false;
    public bool inSugarRush = false;
    public bool onOtherButton = false;
    public int otherButtonID;
    public int otherButtonCode;

    //Scoring
    private bool inScoring = false;
    public GameObject scorecard;
    public TextMeshProUGUI scoreText;
    private int turnCount = 0;
    public int proposedOptimalTurns;
    public int totalCells;
    public int cellsCovered = 0;
    public int[] flavoredCellsCovered = new int[6];
    public int[] desiredPercentages = new int[6];
    public int points = 0;
    public int combo = 1;

    //Dog Text
    public TextMeshProUGUI dogText;
    public string[] dogDialogues;

    public TextMeshProUGUI powerupText;

    public TextMeshProUGUI coverageText;
    public TextMeshProUGUI optimalText;

    public AudioManager am;

    private int piecesPlaced = 0;
    private bool powerUpOnBoard = false;
    public bool hasPowerup = false;
    public bool bypassPlacementRestriction = false;
    public bool srFill = false;
    public int sugarRushID = 0;
    public int powerUpCode = 0;

    public bool canRandomize = true;

    public GameObject mintButton;
    public GameObject nonMintButton;

    //Juice
    public ParticleSystem[] ps;
    public bool[] hasPowerups;
    public int lvlSelSceneID = -1;
    public SpriteRenderer[] powerUpButtons;
    public RawImage reshuffle;
    public Texture2D[] reshuffleButtonImages;
    TutorialSceneManager tsm;
    public TextMeshProUGUI newDogText;
    public TextMeshProUGUI comboText;
    public Animator animReshuffle;

    void Start() {
        Randomize();
        foreach(GameObject border in selectionBorders) {
            border.SetActive(false);
        }
        board = GameObject.Find("Ingredient Layer").GetComponent<Board>();
        turnCount = 1;
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        mintButton.SetActive(false);
        nonMintButton.SetActive(true);
        reshuffle.texture = reshuffleButtonImages[0];
        tsm = GameObject.Find("TutorialManager").GetComponent<TutorialSceneManager>();
    }

    void Update() {

        UpdateCoverageText();
        UpdateReqText();

        if(Input.GetKeyDown(KeyCode.K) && hasPowerup) {
            ExecutePowerUp(powerUpCode);
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(lvlSelSceneID);
        }
        
        if(!inPlacement && !inScoring) {

            for(int i = 0; i < currentCards.Length; i++) {
                if(currentCards[i] != null) {
                    nameText.text = currentCards[currentCardPos].ingredientName;
                    cardImages[i].texture = currentCards[i].image;
                }
            }

            for(int i = 0; i < selectionBorders.Length; i++) {
                if(i == currentCardPos && !onOtherButton) { selectionBorders[i].SetActive(true); }
                else { selectionBorders[i].SetActive(false); }
            }

            if(Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) {
                if(!onOtherButton) {
                    Cursor.visible = false;
                    SelectCard();
                } else {
                    switch(otherButtonID) {
                        case 0:
                            if(!inScoring) {
                                inScoring = true;
                                StartCoroutine(Score());
                            }
                            break;
                        case 1:
                            Randomize();
                            reshuffle.texture = reshuffleButtonImages[1];
                            animReshuffle.Play("shufflever", -1, 0f);
                            canRandomize = false; //COMMENT OUT THIS LINE IF YOU WANT RANDOMIZATION TO BE INFINITE
                            break;
                        case 2:
                            if(hasPowerup) {
                                ExecutePowerUp(powerUpCode);
                            }
                            break;
                        case 3:
                            if(hasPowerups[otherButtonCode]) { ExecutePowerUp(otherButtonCode); }
                            break;
                    }
                    if(otherButtonID == -500) { tsm.BeginTutorial(); }
                }
            } else if (Input.GetKeyDown(KeyCode.X)) {
                Randomize();
            } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                currentCardPos = (currentCardPos + 1) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                currentCardPos = (currentCardPos + currentCards.Length - 1) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                currentCardPos = (currentCardPos + 2) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                currentCardPos = (currentCardPos + currentCards.Length - 2) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                if(!inScoring) {
                    inScoring = true;
                    StartCoroutine(Score());
                }
            }
            
        }
    }

    public void SetButtonActive(int id, bool active) {
        Debug.Log("THIS IS THE CODE: " + id + active);
        if(id < powerUpButtons.Length) {
            float luminance = active ? 255 : 100;
            luminance /= 255;
            powerUpButtons[id].color = new Color(luminance, luminance, luminance, 255);
        }
    }

    public void CheckDinMint() {
        bool dinMintActivate = true;
        for(int i = 0; i < flavoredCellsCovered.Length; i++) {
            if(100 * flavoredCellsCovered[i] / totalCells < desiredPercentages[i]) {
                dinMintActivate = false;
            }
        }
        if(dinMintActivate) {
            powerUpCode = 3;
            SetPowerup(3);
            mintButton.SetActive(true);
            nonMintButton.SetActive(false);
        }
    }

    public void SetPowerup() {
        hasPowerup = true;
        powerupText.fontStyle ^= FontStyles.Italic;
        switch(powerUpCode) {
            case -1:
                powerupText.text = "Powerup: NONE";
                break;
            case 0:
                powerupText.text = "Powerup: OVERLAP";
                break;
            case 1:
                powerupText.text = "Powerup: SUGAR RUSH";
                break;
            case 2:
                powerupText.text = "Powerup: MELT";
                break;
            case 3:
                powerupText.fontStyle = FontStyles.Bold | FontStyles.Italic;
                powerupText.text = "~DINNER MINT~";
                break;
        }
        //SetButtonActive(powerUpCode, true);
    }

    public void SetPowerup(int ID) {
        hasPowerups[ID] = true;
        SetButtonActive(ID, true);
    }

    public void ExecutePowerUp(int code) {
        hasPowerup = false;
        Board b = GameObject.Find("Ingredient Layer").GetComponent<Board>();
        SetButtonActive(code, false);
        switch(code) {
            case 0: //Collapse
                bypassPlacementRestriction = true;
                break;
            case 1: //Sugar Rush
                ZCool();
                //board.SugarRush(sugarRushID);
                srFill = true; //CHANGE TO FILL INSTEAD OF BYPASS
                inSugarRush = true;
                break;
            case 2: //Melt (Look up line-clears in the tutorial i took this from)
                RectInt bounds = b.Bounds;
                int row = -5;
                int attemptCol = 0;
                /*
                for(int col = attemptCol - 1; col < attemptCol + 3; col++) {
                    Vector3Int position = new Vector3Int(col, row, 0);
                    b.setTiles.SetTile(position, null);
                    b.tilemap.SetTile(position, null);
                }*/
                // Shift every row above down one
                for(int i = 0; i < 8; i++) {
                    row = -5;
                    while (row < bounds.yMax)
                    {
                        for (int col = board.minX; col < board.maxX + 1; col++) //Change to be relative to attempt col
                        {
                            Vector3Int ap = new Vector3Int(col, row + 1, 0);
                            TileBase above = b.setTiles.GetTile(ap);

                            Vector3Int position = new Vector3Int(col, row, 0);
                            if(!b.setTiles.HasTile(position) && row >= b.minY) {
                                b.setTiles.SetTile(ap, null);
                                b.tilemap.SetTile(ap, null);
                                b.setTiles.SetTile(position, above);
                            }
                        }

                        row++;
                    }
                }
                break;
            case 3:
                if(!nonMintButton.activeSelf) {
                    b.setTiles.ClearAllTiles();
                    b.tilemap.ClearAllTiles();
                    mintButton.SetActive(false);
                    nonMintButton.SetActive(true);
                    for(int i = 0; i < flavoredCellsCovered.Length; i++) {
                        flavoredCellsCovered[i] = 0;
                        if(desiredPercentages[i] != 0) {
                            desiredPercentages[i] += 5;
                        }
                    }
                    cellsCovered = 0;
                    UpdateCoverageText();
                    foreach(ParticleSystem p in ps) {
                        p.Play();
                    }
                }
                break;
        }
        powerUpCode = -1;
        SetPowerup();
    }

    public void DeselectAll() {
        foreach(GameObject border in selectionBorders) {
            border.SetActive(false);
        }
    }

    public void ResetPowerups() {
        if(inSugarRush) {
            inSugarRush = false;
            srFill = false;
            SetPowerup(1);
        }
        if(bypassPlacementRestriction) {
            bypassPlacementRestriction = false;
            SetPowerup(0);
        }
    }

    public void IncrementPoints(int points, bool holdCombo) {
        combo = holdCombo ? combo + 1 : 1;
        int pointsAdded = points * combo * 1000;
        this.points += pointsAdded;
        dogText.text = "" + this.points;
        comboText.text = "X" + combo;
    }

    public void IncrementPieces() {
        piecesPlaced++;
        if(piecesPlaced % 4 == 0) { //use powerUpOnBoard boolean in this condition to limit powerups on board to one
            board.SpawnPowerup();
            powerUpOnBoard = true;
        }
    }

    private IEnumerator Score() {
        /*
        scoreText.text = "Your Score";
        scorecard.SetActive(true);
        yield return new WaitForSeconds(1f);
        int percentCoveragePoints = (100 * cellsCovered / totalCells) * 10;
        scoreText.text = scoreText.text + "\n\nPercent Covered Points:\n" + percentCoveragePoints;
        yield return new WaitForSeconds(1f);
        int flavorPoints = 1000 - (CalcFd() * 5);
        scoreText.text = scoreText.text + "\n\nFlavor Points:\n" + flavorPoints;
        yield return new WaitForSeconds(1f);
        int turnBonus = (proposedOptimalTurns * 10 - (Mathf.Max(Mathf.Abs(turnCount - proposedOptimalTurns), 0) * 10));
        if (turnCount < proposedOptimalTurns) { turnBonus = proposedOptimalTurns * 10; }
        scoreText.text = scoreText.text + "\n\nTurn Bonus:\n" + turnBonus;
        yield return new WaitForSeconds(1f);
        int totalPoints = percentCoveragePoints + flavorPoints + turnBonus;
        scoreText.text = scoreText.text + "\n\nTotal Points:\n" + totalPoints;
        yield return new WaitForSeconds(1f);
        scoreText.text = scoreText.text + "\n\nOverall Performance:\n";
        yield return new WaitForSeconds(2f);
        int scorePercent = 100 * totalPoints / 1500;
        if(totalPoints == 1500 + proposedOptimalTurns * 10) {
            scoreText.text = scoreText.text + "Perfect!";
        } else if(scorePercent >= 100) {
            scoreText.text = scoreText.text + "Superb!";
        } else if(scorePercent >= 90) {
            scoreText.text = scoreText.text + "Great!";
        } else if(scorePercent >= 80) {
            scoreText.text = scoreText.text + "Good!";
        } else if(scorePercent >= 70) {
            scoreText.text = scoreText.text + "Nice!";
        } else if(scorePercent >= 60) {
            scoreText.text = scoreText.text + "Okay!";
        } else {
            scoreText.text = scoreText.text + "Just Okay.";
        }
        yield return new WaitForSeconds(2f);
        */
        scoreText.text = "Final Score: " + points + "!!!";
        scorecard.SetActive(true);
        yield return new WaitForSeconds(2f);
        if(lvlSelSceneID >= 0) {
            SceneManager.LoadScene(lvlSelSceneID);
        } else {
            Application.Quit();
        }
    }

    //FD for flavor different: returns sum of absolute values of flavor differences at all indices
    private int CalcFd() {
        int sum = 0;
        for(int i = 0; i < desiredPercentages.Length; i++) {
            sum += Mathf.Abs(desiredPercentages[i] - (100 * flavoredCellsCovered[i] / totalCells));
        }
        return sum;
    }

    public void SelectCard() {
        if(!zCooling) {
            ZCool();
            board.SpawnPiece(currentCards[currentCardPos].ingredientId);
            inPlacement = true;
        }
    }

    public void ZCool() {
        zCooling = true;
        StartCoroutine(zCooldown());
    }

    IEnumerator zCooldown() {
        yield return new WaitForSeconds(0.1f);
        zCooling = false;
    }

    public void Randomize() {
        if(canRandomize) {
            reshuffle.texture = reshuffleButtonImages[0];
            List<Card> pullFrom = new List<Card>(); //Use this to disallow duplicates
            for(int i = 0; i < ingredientCards.Length; i++) {
                pullFrom.Add(ingredientCards[i]);
            }
            for(int i = 0; i < currentCards.Length; i++) {
                int randInd = Random.Range(0, pullFrom.Count);
                currentCards[i] = pullFrom[randInd];
                pullFrom.RemoveAt(randInd);
            }
            newDogText.text = dogDialogues[Random.Range(0, dogDialogues.Length)];
            turnCount++;
            turnText.text = "Turn: " + turnCount;
        }
    }

    public void UpdateCoverageText() {
        coverageText.text = "Total: " + (100 * cellsCovered / totalCells) + "%\n\n"
            + "Sweet: " + (100 * flavoredCellsCovered[0] / totalCells) + "%\n"
            + "Rich: " + (100 * flavoredCellsCovered[1] / totalCells) + "%\n"
            + "Plain: " + (100 * flavoredCellsCovered[2] / totalCells) + "%\n"
            + "Tart: " + (100 * flavoredCellsCovered[3] / totalCells) + "%\n"
            + "Salty: " + (100 * flavoredCellsCovered[4] / totalCells) + "%\n"
            + "Spicy: " + (100 * flavoredCellsCovered[5] / totalCells) + "%";
    }
    
    public void UpdateReqText() {
        optimalText.text = "\n\n"
            + "Sweet: " + (desiredPercentages[0]) + "%\n"
            + "Rich: " + (desiredPercentages[1]) + "%\n"
            + "Plain: " + (desiredPercentages[2]) + "%\n"
            + "Tart: " + (desiredPercentages[3]) + "%\n"
            + "Salty: " + (desiredPercentages[4]) + "%\n"
            + "Spicy: " + (desiredPercentages[5]) + "%";
    }

}
