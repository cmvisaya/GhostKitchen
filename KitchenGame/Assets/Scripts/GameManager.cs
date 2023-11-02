using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public bool onOtherButton = false;
    public int otherButtonID;

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

    //Dog Text
    public TextMeshProUGUI dogText;
    public string[] dogDialogues;

    public TextMeshProUGUI coverageText;
    public TextMeshProUGUI optimalText;

    public AudioManager am;

    void Start() {
        Randomize();
        foreach(GameObject border in selectionBorders) {
            border.SetActive(false);
        }
        board = GameObject.Find("Ingredient Layer").GetComponent<Board>();
        turnCount = 1;
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update() {

        UpdateCoverageText();
        UpdateReqText();

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
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
                            break;
                    }
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

    public void DeselectAll() {
        foreach(GameObject border in selectionBorders) {
            border.SetActive(false);
        }
    }

    private IEnumerator Score() {
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
        SceneManager.LoadScene(0);
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
        List<Card> pullFrom = new List<Card>(); //Use this to disallow duplicates
        for(int i = 0; i < ingredientCards.Length; i++) {
            pullFrom.Add(ingredientCards[i]);
        }
        for(int i = 0; i < currentCards.Length; i++) {
            int randInd = Random.Range(0, pullFrom.Count);
            currentCards[i] = pullFrom[randInd];
            pullFrom.RemoveAt(randInd);
        }
        dogText.text = dogDialogues[Random.Range(0, dogDialogues.Length)];
        turnCount++;
        turnText.text = "Turn: " + turnCount;
    }

    public void UpdateCoverageText() {
        if(cellsCovered != 0) {
            coverageText.text = "Total:\n" + (100 * cellsCovered / totalCells) + "%\n"
                + "Sweet:\n" + (100 * flavoredCellsCovered[0] / totalCells) + "%\n"
                + "Rich:\n" + (100 * flavoredCellsCovered[1] / totalCells) + "%\n"
                + "Plain:\n" + (100 * flavoredCellsCovered[2] / totalCells) + "%\n"
                + "Tart:\n" + (100 * flavoredCellsCovered[3] / totalCells) + "%\n"
                + "Salty:\n" + (100 * flavoredCellsCovered[4] / totalCells) + "%\n"
                + "Spicy:\n" + (100 * flavoredCellsCovered[5] / totalCells) + "%";
        }
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
