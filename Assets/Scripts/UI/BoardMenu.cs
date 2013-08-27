using UnityEngine;
using System.Collections;

public class BoardMenu : MonoBehaviour {
	
	public Board board;
	public GUIText boardWidth;
	public GUIText boardHeight;
	public GUIText save;
	public GameObject saveMenu;
	public GameObject loadMenu;
	
	// Receives messages from the buttons
	public void Message(string message) {
		if (message.Equals("SaveBoard")) {
			saveMenu.SetActive(true);
		} else if (message.Equals("LoadBoard")) {
			loadMenu.SetActive(true);
		} else if (message.Equals("Generate")) {
			SetBoardDimensions();
			board.CreateBoard();			
		} else if (message.Equals("Save")) {
			board.SaveBoard(save.text);	
			saveMenu.SetActive(false);
		}  else if (message.Equals("CancelSave")) {
			saveMenu.SetActive(false);
		} else if (message.Equals("CancelLoad")) {
			loadMenu.SetActive(false);
		}
	}
	
	// Parses text input from text boxes
	private void SetBoardDimensions() {
		// Parse width
		if (!boardWidth.text.Equals("")) {
			int w = int.Parse(boardWidth.text.Replace("\b", ""));
			board.boardWidth = w;
		} else {
			board.boardWidth = 1;	
		}
		// Parse height
		if (!boardHeight.text.Equals("")) {
			int h = int.Parse(boardHeight.text.Replace("\b", ""));
			board.boardHeight = h;
		} else {
			board.boardHeight = 1;	
		}	
	}
	
}
