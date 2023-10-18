using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject UI;
    public Card[] ingredientCards;
    public GameObject[] selectionBorders;
    public Card[] currentCards;
    private int currentCardPos = 0;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI turnText;
    public RawImage[] cardImages;
    private Board board;
    public bool zCooling = false;
    public bool inPlacement = false;

    //Scoring
    private bool inScoring = false;
    public GameObject scorecard;
    public TextMeshProUGUI scoreText;
    private int turnCount = 0;
    public int proposedOptimalTurns;
    public int totalCells;
    public int cellsCovered = 0;
    public int[] flavoredCellsCovered = new int[5];
    public int[] desiredPercentages = new int[5];

    //Dog Text
    public TextMeshProUGUI dogText;
    public string[] dogDialogues;

    public TextMeshProUGUI coverageText;

    void Start() {
        Randomize();
        foreach(GameObject border in selectionBorders) {
            border.SetActive(false);
        }
        board = GameObject.Find("Ingredient Layer").GetComponent<Board>();
        turnCount = 1;
    }

    void Update() {
        if(!inPlacement && !inScoring) {

            for(int i = 0; i < currentCards.Length; i++) {
                if(currentCards[i] != null) {
                    nameText.text = currentCards[currentCardPos].ingredientName;
                    cardImages[i].texture = currentCards[i].image;
                }
            }

            for(int i = 0; i < selectionBorders.Length; i++) {
                if(i == currentCardPos) { selectionBorders[i].SetActive(true); }
                else { selectionBorders[i].SetActive(false); }
            }

            if(Input.GetKeyDown(KeyCode.Z)) {
                SelectCard();
            } else if (Input.GetKeyDown(KeyCode.X)) {
                Randomize();
            } else if (Input.GetKeyDown(KeyCode.D)) {
                currentCardPos = (currentCardPos + 1) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.A)) {
                currentCardPos = (currentCardPos + currentCards.Length - 1) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.W)) {
                currentCardPos = (currentCardPos + 2) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.S)) {
                currentCardPos = (currentCardPos + currentCards.Length - 2) % currentCards.Length;
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                StartCoroutine(Score());
            }
            
        }
    }

    private IEnumerator Score() {
        scoreText.text = "Your Score";
        scorecard.SetActive(true);
        yield return new WaitForSeconds(2f);
        int percentCoveragePoints = (100 * cellsCovered / totalCells) * 10;
        scoreText.text = scoreText.text + "\n\nPercent Covered Points:\n" + percentCoveragePoints;
        yield return new WaitForSeconds(2f);
        int flavorPoints = 500 - (CalcFd() * 5);
        scoreText.text = scoreText.text + "\n\nFlavor Points:\n" + flavorPoints;
        yield return new WaitForSeconds(2f);
        int turnBonus = (proposedOptimalTurns * 10 - (Mathf.Max(Mathf.Abs(turnCount - proposedOptimalTurns), 0) * 10));
        if (turnCount < proposedOptimalTurns) { turnBonus = proposedOptimalTurns * 10; }
        scoreText.text = scoreText.text + "\n\nTurn Bonus:\n" + turnBonus;
        yield return new WaitForSeconds(2f);
        int totalPoints = percentCoveragePoints + flavorPoints + turnBonus;
        scoreText.text = scoreText.text + "\n\nTotal Points:\n" + totalPoints;
        yield return new WaitForSeconds(2f);
        scoreText.text = scoreText.text + "\n\nOverall Score:\n";
        yield return new WaitForSeconds(3f);
        int scorePercent = 100 * totalPoints / 1500;
        if(totalPoints == 1500 + proposedOptimalTurns * 10) {
            scoreText.text = scoreText.text + "SS";
        } else if(scorePercent >= 100) {
            scoreText.text = scoreText.text + "S";
        } else if(scorePercent >= 90) {
            scoreText.text = scoreText.text + "A";
        } else if(scorePercent >= 80) {
            scoreText.text = scoreText.text + "B";
        } else if(scorePercent >= 70) {
            scoreText.text = scoreText.text + "C";
        } else if(scorePercent >= 60) {
            scoreText.text = scoreText.text + "D";
        } else {
            scoreText.text = scoreText.text + "F";
        }
    }

    //FD for flavor different: returns sum of  absolute values of flavor differences at all indices
    private int CalcFd() {
        int sum = 0;
        for(int i = 0; i < desiredPercentages.Length; i++) {
            sum += Mathf.Abs(desiredPercentages[i] - (100 * flavoredCellsCovered[i] / totalCells));
        }
        return sum;
    }

    private void SelectCard() {
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
        for(int i = 0; i < currentCards.Length; i++) {
            currentCards[i] = ingredientCards[Random.Range(0, ingredientCards.Length)];
        }
        dogText.text = dogDialogues[Random.Range(0, dogDialogues.Length)];
        UpdateCoverageText();
        turnCount++;
        turnText.text = "Turn: " + turnCount;
    }

    private void UpdateCoverageText() {
        if(cellsCovered != 0) {
            coverageText.text = "Total:\n" + (100 * cellsCovered / totalCells) + "%\n\n"
                + "Sweet:\n" + (100 * flavoredCellsCovered[0] / totalCells) + "%\n\n"
                + "Rich:\n" + (100 * flavoredCellsCovered[1] / totalCells) + "%\n\n"
                + "Plain:\n" + (100 * flavoredCellsCovered[2] / totalCells) + "%\n\n"
                + "Tart:\n" + (100 * flavoredCellsCovered[3] / totalCells) + "%\n\n"
                + "Salty:\n" + (100 * flavoredCellsCovered[4] / totalCells) + "%";
        }
    }

}
