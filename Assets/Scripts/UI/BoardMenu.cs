using UnityEngine;
using System.Collections;

public class BoardMenu : MonoBehaviour {
	
	public Board board;
	public GUIText boardWidth;
	public GUIText boardHeight;
	
	// Receives messages from the buttons
	public void Message(string message) {
		if (message.Equals("Save")) {
			board.SaveBoard();	
		} else if (message.Equals("Load")) {
			board.LoadBoard();	
		} else if (message.Equals("Generate")) {
			SetBoardDimensions();
			board.CreateBoard();			
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
