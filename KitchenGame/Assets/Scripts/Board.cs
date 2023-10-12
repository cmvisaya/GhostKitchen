using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tilemap boardMap { get; private set; }
    public Tilemap setTiles { get; private set; }
    public Piece activePiece {get; private set; }
    public IngredientData[] ingredients;
    public Vector3Int spawnPosition;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.boardMap = GameObject.Find("Board Layer").GetComponentInChildren<Tilemap>();
        this.setTiles = GameObject.Find("Set Tiles").GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        
        for (int i = 0; i < this.ingredients.Length; i++) {
            this.ingredients[i].Initialize();
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
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.setTiles.SetTile(tilePosition, piece.data.tile);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position) {
        for(int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!this.boardMap.HasTile(tilePosition) || this.setTiles.HasTile(tilePosition)) {
                return false;
            }
        }
        return true;
    }


}
