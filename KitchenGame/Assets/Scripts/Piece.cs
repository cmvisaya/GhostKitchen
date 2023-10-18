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

            if(Input.GetKeyDown(KeyCode.Q)) {
                Rotate(-1);
            } else if (Input.GetKeyDown(KeyCode.E)) {
                Rotate(1);
            }

            if(Input.GetButtonDown("Up")) {
                Move(Vector2Int.up);
            } else if (Input.GetButtonDown("Down")) {
                Move(Vector2Int.down);
            } else if (Input.GetButtonDown("Left")) {
                Move(Vector2Int.left);
            } else if (Input.GetButtonDown("Right")) {
                Move(Vector2Int.right);
            }

            this.board.Set(this);

            if(Input.GetKeyDown(KeyCode.Z)) {
                Lock();
            }
            if(Input.GetKeyDown(KeyCode.X)) {
                Cancel();
            }
        }
        
    }

    private void Move(Vector2Int translation) {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);
        valid = true; //Comment this out to prevent movement outside of bounds
        if (valid) {
            this.position = newPosition;
        }
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
    }

    private void Lock() {
        if(!gm.zCooling) {
            gm.ZCool();
            bool valid = this.board.IsValidPosition(this, this.position);

            if(valid) {
                gm.cellsCovered += cellCount;
                switch(this.data.flavor) {
                    case Flavors.SWEET:
                        gm.flavoredCellsCovered[0] += cellCount;
                        break;
                    case Flavors.RICH:
                        gm.flavoredCellsCovered[1] += cellCount;
                        break;
                    case Flavors.PLAIN:
                        gm.flavoredCellsCovered[2] += cellCount;
                        break;
                    case Flavors.TART:
                        gm.flavoredCellsCovered[3] += cellCount;
                        break;
                    case Flavors.SALTY:
                        gm.flavoredCellsCovered[4] += cellCount;
                        break;
                }
                gm.inPlacement = false;
                gm.Randomize();
                this.board.Lock(this);
            } else {
                Debug.Log("SPOT INVALID!");
            }
        }
    }
}
