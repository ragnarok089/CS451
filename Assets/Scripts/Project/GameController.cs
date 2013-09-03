using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public Board board;
	public GUITexture player1Icon;
	public GUITexture player2Icon;
	public Texture player1Default;
	public Texture player1Selected;
	public Texture player2Default;
	public Texture player2Selected;
	
	public Player player1;
	public Player player2;
	
	public GameObject timer;
	public GameObject selectedLight;	
	
	
	private bool player1Turn = true;
	private bool pieceSelected = false;
	private Piece selectedPiece;
	
	
	private GUILayer gui;
	private Vector3 mousePos;
	private Vector3 worldPos;

	// Use this for initialization
	void Start () {
		gui = Camera.main.GetComponent<GUILayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			Application.LoadLevel("MainMenu");	
		}
		
		if (player1Turn) {
			player1Icon.texture = player1Selected;
			player2Icon.texture = player2Default;
		} else {
			player1Icon.texture = player1Default;
			player2Icon.texture = player2Selected;	
		}
		
		if(Input.GetMouseButtonDown(0) && gui.HitTest(Input.mousePosition) == null) {
			mousePos = Input.mousePosition;
			worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, this.transform.position.y));
			selectedLight.transform.position = new Vector3(Mathf.Round(worldPos.x), selectedLight.transform.position.y, Mathf.Round(worldPos.z));
			if (!pieceSelected) {
				SelectPiece((int)Mathf.Round(worldPos.x), (int)Mathf.Round(worldPos.z));
				
			} else {
				MovePiece((int)Mathf.Round(worldPos.x), (int)Mathf.Round(worldPos.z));
			}
		}
	}
	
	void SelectPiece(int x, int y) {
		if (board.GetTile(x, y) != null) {
			if (board.GetTile(x, y).piece != null) {
				//if (ValidatePiece(board.GetTile(x, y).piece)) {
					selectedPiece = board.GetTile(x, y).piece;
					pieceSelected = true;
				//}
			}
		}
		
	}
	
	void MovePiece(int x, int y) {
		if (board.GetTile(x, y) != null) {
			if (ValidateMove(board.GetTile(x, y))) {
				selectedPiece.transform.position = new Vector3(x, 0, y);
				board.GetTile(x, y).piece = selectedPiece;
				pieceSelected = false;
				player1Turn = !player1Turn;
			}
		}
		
	}
	
	// Checks to see if the piece is owned by the correct player
	private bool ValidatePiece(Piece p) {
		bool isValid = false;
		
		if (player1Turn) {
			foreach (Piece playerPiece in player1.piece) {
				if (playerPiece.transform.position.Equals(p.transform.position)) {
					isValid = true;
				}	
			}
		} else {
			foreach (Piece playerPiece in player2.piece) {
				if (playerPiece.transform.position.Equals(p.transform.position)) {
					isValid = true;
				}	
			}
		}
		return isValid;
	}
	
	
	private bool ValidateMove(Tile t) {
		bool isValid = true;
		/*
		if (selectedPiece.moves.Contains("right")) {
			if ((Mathf.Round(t.transform.position.x) - Mathf.Round(selectedPiece.transform.position.x) == 1)) {
				isValid = true;
			}
		} else if (selectedPiece.moves.Contains("left")) {
			if ((Mathf.Round(t.transform.position.x) - Mathf.Round(selectedPiece.transform.position.x) == -1)) {
				isValid = true;
			}
		} else if (selectedPiece.moves.Contains("backward")) {
			if ((Mathf.Round(t.transform.position.z) - Mathf.Round(selectedPiece.transform.position.z) == -1)) {
				isValid = true;
			}
		} else if (selectedPiece.moves.Contains("forward")) {
			if ((Mathf.Round(t.transform.position.z) - Mathf.Round(selectedPiece.transform.position.z) == 1)) {
				isValid = true;
			}
		}
		*/
		return isValid;	
	}
}
