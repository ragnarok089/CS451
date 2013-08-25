using UnityEngine;
using System.Collections;
using System.IO;

public class TileMenu : MonoBehaviour {

	public Board board;
	public GUIText xPos;
	public GUIText yPos;
	public GameObject invalidTile;
	public GameObject[] icons;
	private Tile currentTile;
	public System.Collections.Generic.List<Texture2D> textures;
	private int page = 0;
	
	// Use this for initialization
	void Start () {
		currentTile = board.GetTile(0, 0);
		LoadTexturesFromFolder();
		ShowIcons(page);
	}
	
	// Handles messages coming from the buttons
	public void Message(string message) {
		if (message.Equals("Select")) { // Select button
			currentTile = board.GetTile(int.Parse(xPos.text.Replace("\b", "")), int.Parse(yPos.text.Replace("\b", "")));
			if (currentTile == null) {
				invalidTile.SetActive(true);		
			} else {
				invalidTile.SetActive(false);	
			}
		} else if (message.Equals("Next")) {  // Next button
			if (page < (textures.Count / icons.Length)) {
				page++;
				ShowIcons(page);
			}
		} else if (message.Equals("Previous")) {  // Previous button
			if (page > 0) {
				page--;
				ShowIcons(page);
			}
		// Image icon buttons
		} else if (message.Equals("Icon1") && currentTile != null) {
			currentTile.SetTexture(icons[0].guiTexture.texture);	
		} else if (message.Equals("Icon2") && currentTile != null) {
			currentTile.SetTexture(icons[1].guiTexture.texture);
		} else if (message.Equals("Icon3") && currentTile != null) {
			currentTile.SetTexture(icons[2].guiTexture.texture);
		} else if (message.Equals("Icon4") && currentTile != null) {
			currentTile.SetTexture(icons[3].guiTexture.texture);
		} else if (message.Equals("Icon5") && currentTile != null) {
			currentTile.SetTexture(icons[4].guiTexture.texture);
		} else if (message.Equals("Icon6") && currentTile != null) {
			currentTile.SetTexture(icons[5].guiTexture.texture);
		}
	}
	
	// Displays 6 of the textures in the textures list
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
		string[] filePaths = Directory.GetFiles(Application.dataPath + "/Resources/Tiles/");
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
}
