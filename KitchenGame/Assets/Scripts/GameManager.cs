using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject UI;
    public Card[] ingredientCards;
    public Card currentCard;
    private int currentCardPos = 0;
    public TextMeshProUGUI nameText;
    private Board board;
    public bool zCooling = false;
    public bool inPlacement = false;

    void Start() {
        board = GameObject.Find("Ingredient Layer").GetComponent<Board>();
        currentCard = ingredientCards[currentCardPos];
    }

    void Update() {
        if(UI.activeSelf) {

            if(currentCard != null) {
                nameText.text = currentCard.ingredientName;
            }

            if(Input.GetKeyDown(KeyCode.Z)) {
                SelectCard();
            } else if (Input.GetKeyDown(KeyCode.D)) {
                currentCardPos = (currentCardPos + 1) % ingredientCards.Length;
                currentCard = ingredientCards[currentCardPos];
            } else if (Input.GetKeyDown(KeyCode.A)) {
                currentCardPos = (currentCardPos + ingredientCards.Length - 1) % ingredientCards.Length;
                currentCard = ingredientCards[currentCardPos];
            }
        }
    }

    private void SelectCard() {
        if(!zCooling) {
            ZCool();
            board.SpawnPiece(currentCard.ingredientId);
            UI.SetActive(false);
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

}
