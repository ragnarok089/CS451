using UnityEngine;
using System.Collections;
using System.IO;

public class PieceMenu : MonoBehaviour {

	public Board board;
	public GUIText xPos;
	public GUIText yPos;
	public GameObject invalidLocation;
	public GameObject invalidPiece;
	public GameObject[] icons;
	public GameObject[] checkBoxes;
	public System.Collections.Generic.List<string> moves;
	private Piece currentPiece;
	public System.Collections.Generic.List<Texture2D> textures;
	private int page = 0;
	private string selectedDirection = "forward";
	private bool forward = false;
	private bool backward = false;
	private bool left = false;
	private bool right = false;
	private bool forwardLeft = false;
	private bool forwardRight = false;
	private bool backwardLeft = false;
	private bool backwardRight = false;
	
	// Use this for initialization
	void Start () {
		LoadTexturesFromFolder();
		ShowIcons(page);
	}
	
	// Handles messages coming from the different buttons
	public void Message(string message) {
		if (message.Equals("Select")) {
			Tile t = board.GetTile(int.Parse(xPos.text.Replace("\b", "")), int.Parse(yPos.text.Replace("\b", "")));
			invalidPiece.SetActive(false);
			invalidLocation.SetActive(false);
			if (t == null) {
				invalidLocation.SetActive(true);	
			} else {
				invalidLocation.SetActive(false);	
				if (t.piece == null) {
					invalidPiece.SetActive(true);		
				} else {
					currentPiece = t.piece;
					GetMoves();
					UpdateCheckboxes();
					invalidPiece.SetActive(false);	
				}
			}
		} else if (message.Equals("Create")) {
			Tile t = board.GetTile(int.Parse(xPos.text.Replace("\b", "")), int.Parse(yPos.text.Replace("\b", "")));
			invalidPiece.SetActive(false);
			invalidLocation.SetActive(false);
			if (t == null) {
				invalidLocation.SetActive(true);	
			} else {
				invalidLocation.SetActive(false);	
				board.AddPiece(int.Parse(xPos.text.Replace("\b", "")), int.Parse(yPos.text.Replace("\b", "")));	
				currentPiece = board.GetTile(int.Parse(xPos.text.Replace("\b", "")), int.Parse(yPos.text.Replace("\b", ""))).piece;
				currentPiece.SetDirection(selectedDirection);
				SetMoves();
				UpdateCheckboxes();
			}
		} else if (message.Equals("Delete")) {
			Tile t = board.GetTile(int.Parse(xPos.text.Replace("\b", "")), int.Parse(yPos.text.Replace("\b", "")));
			invalidPiece.SetActive(false);
			invalidLocation.SetActive(false);
			if (t == null) {
				invalidLocation.SetActive(true);	
			} else {
				invalidLocation.SetActive(false);	
				if (t.piece == null) {
					invalidPiece.SetActive(true);		
				} else {
					Destroy(t.piece.gameObject);
					invalidPiece.SetActive(false);	
				}
			}
				
		} else if (message.Equals("OrientationForward")) {
			selectedDirection = "forward";
			if (currentPiece != null) {
				currentPiece.SetDirection("forward");
			}
		} else if (message.Equals("OrientationBackward")) {
			selectedDirection = "backward";
			if (currentPiece != null) {
				currentPiece.SetDirection("backward");	
			}
		} else if (message.Equals("OrientationLeft")) {
			selectedDirection = "left";
			if (currentPiece != null) {	
				currentPiece.SetDirection("left");
			}
		} else if (message.Equals("OrientationRight")) {
			selectedDirection = "right";
			if (currentPiece != null) {
				currentPiece.SetDirection("right");
			}
		} else if (message.Contains("Unchecked")) {
			RemoveMove(message.Substring(0, message.IndexOf("Unchecked")));	
		} else if (message.Contains("Checked")) {
			AddMove(message.Substring(0, message.IndexOf("Checked")));	
		}
		
		/*else if (message.Equals("ForwardChecked")) {
			forward = true;
			UpdateMoves();
		} else if (message.Equals("ForwardUnchecked")) {
			forward = false;
			UpdateMoves();
		} else if (message.Equals("BackwardChecked")) {
			backward = true;
			UpdateMoves();
		} else if (message.Equals("BackwardUnchecked")) {
			backward = false;
			UpdateMoves();
		} else if (message.Equals("LeftChecked")) {
			left = true;
			UpdateMoves();
		} else if (message.Equals("LeftUnchecked")) {
			left = false;
			UpdateMoves();
		} else if (message.Equals("RightChecked")) {
			right = true;
			UpdateMoves();
		} else if (message.Equals("RightUnchecked")) {
			right = false;
			UpdateMoves();
		} else if (message.Equals("ForwardLeftChecked")) {
			forwardLeft = true;
			UpdateMoves();
		} else if (message.Equals("ForwardLeftUnchecked")) {
			forwardLeft = false;
			UpdateMoves();
		} else if (message.Equals("ForwardRightChecked")) {
			forwardRight = true;
			UpdateMoves();
		} else if (message.Equals("ForwardRightUnchecked")) {
			forwardRight = false;
			UpdateMoves();
		} else if (message.Equals("BackwardLeftChecked")) {
			backwardLeft = true;
			UpdateMoves();
		} else if (message.Equals("BackwardLeftUnchecked")) {
			backwardLeft = false;
			UpdateMoves();
		}  else if (message.Equals("BackwardRightChecked")) {
			backwardRight = true;
			UpdateMoves();
		} else if (message.Equals("BackwardRightUnchecked")) {
			backwardRight = false;
			UpdateMoves();
		} 
		*/
		else if (message.Equals("Icon1") && currentPiece != null) {
			currentPiece.SetTexture(icons[0].guiTexture.texture);	
		} else if (message.Equals("Icon2") && currentPiece != null) {
			currentPiece.SetTexture(icons[1].guiTexture.texture);
		} else if (message.Equals("Icon3") && currentPiece != null) {
			currentPiece.SetTexture(icons[2].guiTexture.texture);
		}
	}
	
	// Displays 3 of the textures in the textures list
	private void ShowIcons(int page) {
		foreach (GameObject g in icons) {
			g.SetActive(false);
		}
		
		int count = 0;
		for (int i = page * icons.Length; i < textures.Count; i++, count++ ) {
			if (count >= icons.Length) {
				i = textures.Count;
				break;
			} else {
				icons[count].SetActive(true);
				icons[count].guiTexture.texture = textures[i];
			}
		}
	}
	
	// Loads textures from the Resources folder and store them in the textures list
	private void LoadTexturesFromFolder() {
		string[] filePaths = Directory.GetFiles(Application.dataPath + "/Resources/Pieces/");
		for (int i = 0; i < filePaths.Length; i++) {
			if (filePaths[i].ToLower().Contains(".jpg") || filePaths[i].ToLower().Contains(".png")) {
				Texture2D texture = new Texture2D(4,4);
				FileStream fs = new FileStream(filePaths[i], FileMode.Open, FileAccess.Read);
				byte[] imageData = new byte[fs.Length];
				fs.Read(imageData, 0, (int) fs.Length);
				texture.LoadImage(imageData);
				textures.Add(texture);	
			}
		}
	}
	
	// Gets the list of moves from a piece
	private void GetMoves() {
		if (currentPiece != null) {
			moves = new System.Collections.Generic.List<string>();
			for (int i = 0; i < currentPiece.moves.Count; i++) {
				moves.Add(currentPiece.moves[i]);	
			}
		}
	}
	
	// Sets the list of moves for a piece
	private void SetMoves() {
		if (currentPiece != null) {
			currentPiece.moves = new System.Collections.Generic.List<string>();
			for (int i = 0; i < moves.Count; i++) {
				currentPiece.moves.Add(moves[i]);	
			}
		}
	}
	
	// Adds a move to the moves list
	private void AddMove(string direction) {
		bool alreadyExists = false;
		for (int i = 0; i < moves.Count; i++) {
			if (moves[i].Equals(direction)) {
				alreadyExists = true;
				break;
			}
		}
		if (!alreadyExists) {
			moves.Add(direction);	
		}
		if (currentPiece != null) {
			SetMoves();	
		}
	}
	
	// Removes a move from the moves list
	private void RemoveMove(string direction) {
		for (int i = 0; i < moves.Count; i++) {
			if (moves[i].Equals(direction)) {
				moves.RemoveAt(i);
				break;
			}
		}
		if (currentPiece != null) {
			SetMoves();	
		}
	}
	
	// Updates checkboxes based on moves list
	private void UpdateCheckboxes() {
		for (int i = 0; i < checkBoxes.Length; i++) {
			checkBoxes[i].SendMessage("Select", false);	
		}
		
		for (int i = 0; i < moves.Count; i++) {
			if (moves[i].ToLower().Equals("forward")) {
				checkBoxes[0].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("backward")) {
				checkBoxes[1].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("left")) {
				checkBoxes[2].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("right")) {
				checkBoxes[3].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("forwardleft")) {
				checkBoxes[4].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("forwardright")) {
				checkBoxes[5].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("backwardleft")) {
				checkBoxes[6].SendMessage("Select", true);	
			} else if (moves[i].ToLower().Equals("backwardright")) {
				checkBoxes[7].SendMessage("Select", true);	
			}
		}	
	}
}
