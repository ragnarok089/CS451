using UnityEngine;
using System.Collections;

public class EditMenu : MonoBehaviour {

	public GameObject boardMenu;
	public GameObject tileMenu;
	public GameObject pieceMenu;
	public GameObject playerMenu;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			Application.LoadLevel("MainMenu");	
		}
	}
	
	// This message function are how the buttons communicate with this class
	public void Message(string message) {
		// Change the menu based on the message from the button
		if (message.Equals("Board")) {
			HideMenus();
			boardMenu.SetActive(true);
		} else if (message.Equals("Tile")) {
			HideMenus();
			tileMenu.SetActive(true);
		} else if (message.Equals("Piece")) {
			HideMenus();
			pieceMenu.SetActive(true);
		} else if (message.Equals("Player")) {
			HideMenus();
			playerMenu.SetActive(true);
		}
	}
	
	// Disables all menus
	private void HideMenus() {
		boardMenu.SetActive(false);
		tileMenu.SetActive(false);
		pieceMenu.SetActive(false);
		playerMenu.SetActive(false);
	}
}
