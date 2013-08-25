﻿using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public int boardWidth = 10;
	public int boardHeight = 10;
	public Tile[] tiles;
	public Tile tileTemplate;
	public Piece pieceTemplate;

	// Use this for initialization
	void Start () {
		CreateBoard();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Returns a tile at a given position
	public Tile GetTile(int x, int y) {
		for (int i = 0; i < tiles.Length; i++) {
			if ((int)tiles[i].transform.position.x == x && (int)tiles[i].transform.position.z == y)	{
				return tiles[i];	
			}
		}
		return null;
	}
	
	// Replaces a tile at a given position
	public void SetTile(Tile t) {
		for (int i = 0; i < tiles.Length; i++) {
			if ((int)tiles[i].transform.position.x == (int)t.transform.position.x && (int)tiles[i].transform.position.z == (int)t.transform.position.z)	{
				tiles[i] = t;
				break;
			}
		}	
	}
	
	// Adds a piece to a tile.  Destroys current piece, if any
	public void AddPiece(int x, int y) {
		for (int i = 0; i < tiles.Length; i++) {
			if ((int)tiles[i].transform.position.x == x && (int)tiles[i].transform.position.z == y)	{
				if (tiles[i].piece != null) {
					Destroy (tiles[i].piece.gameObject);
				}
				tiles[i].piece = (Piece)Instantiate(pieceTemplate, new Vector3(x, 0, y), pieceTemplate.transform.rotation);	
			}
		}	
	}
	
	// Creates a game board with the specified width and height.  Uses generic tiles
	public void CreateBoard() {
		ClearBoard();
		tiles = new Tile[boardWidth * boardHeight];
		for (int i = 0; i < boardWidth; i++) {
			for (int j = 0; j < boardHeight; j++) {
				tiles[(i * boardHeight) + j] = (Tile)Instantiate(tileTemplate, new Vector3(i, 0, j), tileTemplate.transform.rotation);
			}
		}
	}
	
	// Saves the board, tiles and pieces
	public void SaveBoard() {
		print ("Save Board");
		SaveData save = new SaveData();
		save.boardHeight = boardHeight;
		save.boardWidth = boardWidth;
		save.tiles = tiles;
		SaveLoad obj = new SaveLoad();
		
	}
	
	// Loads the board from a file
	public void LoadBoard() {
		print ("Load Board");
		
	}
	
	// Clears all tiles and pieces from the board
	private void ClearBoard() {
		for (int i = 0; i < tiles.Length; i++) {
			Destroy(tiles[i].gameObject);
			if (tiles[i].piece != null) {
				Destroy(tiles[i].piece.gameObject);	
			}
		}
	}
}