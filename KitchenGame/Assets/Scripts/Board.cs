using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tilemap boardMap { get; private set; }
    public Tilemap setTiles { get; private set; }
    public Tilemap powerUpTiles {get; private set; }
    public Tilemap lastPlacedTiles {get; private set; }
    public Piece activePiece {get; private set; }
    public IngredientData[] ingredients;
    public IngredientData[] sugarRushIngredients;
    public Vector3Int spawnPosition;

    public Vector3Int[] oldTilePositions;
    public Flavors oldFlavor;

    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    public int minY = 0;
    public int maxY = 0;
    public int minX = 0;
    public int maxX = 0;
    public Tile powerTile;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.boardMap = GameObject.Find("Board Layer").GetComponentInChildren<Tilemap>();
        this.setTiles = GameObject.Find("Set Tiles").GetComponentInChildren<Tilemap>();
        this.powerUpTiles = GameObject.Find("Powerup Layer").GetComponentInChildren<Tilemap>();
        this.lastPlacedTiles = GameObject.Find("Zones Tiles").GetComponentInChildren<Tilemap>();
        lastPlacedTiles.ClearAllTiles();
        this.activePiece = GetComponentInChildren<Piece>();
        
        for (int i = 0; i < this.ingredients.Length; i++) {
            this.ingredients[i].Initialize();
        }

        for (int i = 0; i < this.sugarRushIngredients.Length; i++) {
            this.sugarRushIngredients[i].Initialize();
        }
        
    }

    private void Start() {
        //SpawnPiece(1);
    }

    public void SpawnPiece(int id) {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gm.inSugarRush) {
            IngredientData data = this.ingredients[id];
            switch(data.flavor) {
                case Flavors.SWEET:
                    SugarRush(0);
                    break;
                case Flavors.RICH:
                    SugarRush(1);
                    break;
                case Flavors.PLAIN:
                    SugarRush(2);
                    break;
                case Flavors.TART:
                    SugarRush(3);
                    break;
                case Flavors.SALTY:
                    SugarRush(4);
                    break;
                case Flavors.SPICY:
                    SugarRush(5);
                    break;
            }
        } else {
            IngredientData data = this.ingredients[id];
            this.activePiece.Initialize(this, this.spawnPosition, data);
            Set(this.activePiece);
        }
    }

    public void SugarRush(int id) {
        IngredientData data = this.sugarRushIngredients[id];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void Lock(Piece piece) {
        bool foundAdjacent = false;
        bool overlapPowerup = false;
        int cellsDown = 0;
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        Vector3Int[] newTilePositions = new Vector3Int[piece.cells.Length];
        this.lastPlacedTiles.ClearAllTiles();
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            newTilePositions[i] = tilePosition;
            if(!gm.srFill || (gm.srFill && this.boardMap.HasTile(tilePosition) && !this.setTiles.HasTile(tilePosition))) {
                this.setTiles.SetTile(tilePosition, piece.data.tile);
                this.lastPlacedTiles.SetTile(tilePosition, piece.data.tile);
                cellsDown++;
            }
            this.tilemap.SetTile(tilePosition, null);
            if(this.powerUpTiles.HasTile(tilePosition)) {
                this.powerUpTiles.SetTile(tilePosition, null);
                overlapPowerup = true;
            }
            if(oldTilePositions != null && !foundAdjacent) {
                foreach(Vector3Int oldTilePos in oldTilePositions) {
                    Vector3Int[] directions = {new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(-1, 0, 0)};
                    foreach(Vector3Int dir in directions) {
                        if(tilePosition + dir == oldTilePos) {
                            foundAdjacent = true;
                            break;
                        }
                    }
                    if(foundAdjacent) {break;}
                }
            }
        }
        Debug.Log("Found adjacent: " + foundAdjacent + " | " + piece.data.flavor + " | " + oldFlavor);
        bool holdCombo = foundAdjacent && (piece.data.flavor == oldFlavor);
        gm.IncrementPoints(cellsDown, holdCombo);
        oldTilePositions = new Vector3Int[newTilePositions.Length];
        oldFlavor = piece.data.flavor;
        Array.Copy(newTilePositions, oldTilePositions, newTilePositions.Length);
        int pcode = -1;
        if(overlapPowerup) {
            switch(oldFlavor) {
                case Flavors.SWEET:
                        gm.powerUpCode = 1;
                        pcode = 1;
                        gm.hasPowerups[1] = true;
                        break;
                    case Flavors.RICH:
                        gm.powerUpCode = 2;
                        pcode = 2;
                        gm.hasPowerups[2] = true;
                        break;
                    case Flavors.PLAIN:
                        gm.powerUpCode = 1;
                        pcode = 1;
                        gm.hasPowerups[1] = true;
                        break;
                    case Flavors.TART:
                        gm.powerUpCode = 0;
                        pcode = 0;
                        gm.hasPowerups[0] = true;
                        break;
                    case Flavors.SALTY:
                        gm.powerUpCode = 0;
                        pcode = 0;
                        gm.hasPowerups[0] = true;
                        break;
                    case Flavors.SPICY:
                        gm.powerUpCode = 2;
                        pcode = 2;
                        gm.hasPowerups[2] = true;
                        break;
            }
            gm.SetPowerup(pcode);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position, bool ignoreSet, bool srFill) {
        for(int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!srFill && (!this.boardMap.HasTile(tilePosition) || (this.setTiles.HasTile(tilePosition) && !ignoreSet))) {
                return false;
            }
        }
        return true;
    }

    public void SpawnPowerup() {
        bool validPowerPosition = false;
        Vector3Int newPos;
        int tries = 1000000;
        do {
            newPos = new Vector3Int(UnityEngine.Random.Range(minX, maxX + 1), UnityEngine.Random.Range(minY, maxY + 1), 0);
            validPowerPosition = this.boardMap.HasTile(newPos) && !this.setTiles.HasTile(newPos);
            tries--;
        } while (!validPowerPosition);
        this.powerUpTiles.SetTile(newPos, powerTile);
    }
}
