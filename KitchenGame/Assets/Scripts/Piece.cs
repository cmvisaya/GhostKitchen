using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board {get; private set;}
    public IngredientData data {get; private set;}
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    private GameManager gm;
    private int cellCount;

    public void Initialize(Board board, Vector3Int position, IngredientData data) {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.cells = new Vector3Int[data.cells.Length];

        for(int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }

        cellCount = data.cells.Length;
    }

    private void Update() {

        if(this.board != null && gm.inPlacement) {
            this.board.Clear(this);

            bool valid = this.board.IsValidPosition(this, this.position, gm.bypassPlacementRestriction, gm.srFill);

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if(Input.GetKeyDown(KeyCode.Q) || scroll > 0) {
                Rotate(-1);
            } else if (Input.GetKeyDown(KeyCode.E) || scroll < 0) {
                Rotate(1);
            }

            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");
            if(Input.GetButtonDown("Up")) {
                Move(Vector2Int.up);
            } else if (Input.GetButtonDown("Down")) {
                Move(Vector2Int.down);
            } else if (Input.GetButtonDown("Left")) {
                Move(Vector2Int.left);
            } else if (Input.GetButtonDown("Right")) {
                Move(Vector2Int.right);
            } else if(Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0) {
                Move();
            } 

            this.board.Set(this);

            if(Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1")) {
                Cursor.visible = true;
                Lock();
            }
            if(Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Fire2")) {
                Cursor.visible = true;
                Cancel();
            }
        }
        
    }

    private void Move(Vector2Int translation) {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        this.position = newPosition;
    }

    private void Move() {
        Cursor.visible = false;
        Vector3Int newPosition = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        newPosition.z = 0;
        Debug.Log(newPosition);
        this.position = newPosition;
    }

    private void Rotate(int direction) {
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        for(int i = 0; i < this.cells.Length; i++) {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.ingredient) {
                /*case Ingredients.SQUARE:
                    cell.x += 0.5f;
                    cell.y += 0.5f;
                    x = Mathf.FloorToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.FloorToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                case Ingredients.CHOC_CHIP:
                    cell.x += 0.5f;
                    cell.y += 0.5f;
                    x = Mathf.FloorToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.FloorToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;*/
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private int Wrap(int input, int min, int max) {
        if(input < min) {
            return max - (min - input) % (max - min);
        }
        else {
            return min + (input - min) % (max - min);
        }
    }

    private void Cancel() {
        this.board.Clear(this);
        gm.inPlacement = false;
        gm.UI.SetActive(true);
        gm.ResetPowerups();
    }

    private void Lock() {
        if(!gm.zCooling) {
            gm.ZCool();
            bool valid = this.board.IsValidPosition(this, this.position, gm.bypassPlacementRestriction, gm.srFill);

            if(valid) {
                gm.cellsCovered += cellCount;
                switch(this.data.flavor) {
                    case Flavors.SWEET:
                        gm.flavoredCellsCovered[0] += cellCount;
                        gm.ps[0].Play();
                        break;
                    case Flavors.RICH:
                        gm.flavoredCellsCovered[1] += cellCount;
                        gm.ps[1].Play();
                        break;
                    case Flavors.PLAIN:
                        gm.flavoredCellsCovered[2] += cellCount;
                        gm.ps[2].Play();
                        break;
                    case Flavors.TART:
                        gm.flavoredCellsCovered[3] += cellCount;
                        gm.ps[3].Play();
                        break;
                    case Flavors.SALTY:
                        gm.flavoredCellsCovered[4] += cellCount;
                        gm.ps[4].Play();
                        break;
                    case Flavors.SPICY:
                        gm.flavoredCellsCovered[5] += cellCount;
                        gm.ps[5].Play();
                        break;
                }
                gm.inPlacement = false;
                gm.bypassPlacementRestriction = false;
                gm.srFill = false;
                gm.inSugarRush = false;
                gm.canRandomize = true;
                gm.Randomize();
                this.board.Lock(this);
                gm.am.Play(1, 1f);
                gm.IncrementPieces();
                gm.CheckDinMint();
            } else {
                gm.am.Play(0, 0.5f);
            }
        }
    }
}
