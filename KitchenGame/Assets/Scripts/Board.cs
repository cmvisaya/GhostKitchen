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
        IngredientData data = this.ingredients[id];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
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
        Vector3Int[] newTilePositions = new Vector3Int[piece.cells.Length];
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            newTilePositions[i] = tilePosition;
            this.setTiles.SetTile(tilePosition, piece.data.tile);
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
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncrementPoints(piece.cells.Length, holdCombo);
        oldTilePositions = new Vector3Int[newTilePositions.Length];
        oldFlavor = piece.data.flavor;
        Array.Copy(newTilePositions, oldTilePositions, newTilePositions.Length);
        if(overlapPowerup) {
            switch(oldFlavor) {
                case Flavors.SWEET:
                        gm.powerUpCode = 1;
                        break;
                    case Flavors.RICH:
                        gm.powerUpCode = 2;
                        break;
                    case Flavors.PLAIN:
                        gm.powerUpCode = 1;
                        break;
                    case Flavors.TART:
                        gm.powerUpCode = 0;
                        break;
                    case Flavors.SALTY:
                        gm.powerUpCode = 0;
                        break;
                    case Flavors.SPICY:
                        gm.powerUpCode = 2;
                        break;
            }
            gm.SetPowerup();
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position, bool ignoreSet) {
        for(int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!this.boardMap.HasTile(tilePosition) || (this.setTiles.HasTile(tilePosition) && !ignoreSet)) {
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
