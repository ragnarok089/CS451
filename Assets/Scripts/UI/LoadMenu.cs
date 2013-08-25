using UnityEngine;
using System.Collections;
using System.IO;

public class LoadMenu : MonoBehaviour {
	
	public GameObject[] fileButtons;
	public GameObject[] fileNames;
	public System.Collections.Generic.List<string> files;
	private int page = 0;
	
	
	void Start() {
		LoadBoardsFromFolder();
		ShowBoards(page);
	}
	
	public void Message(string message) { 
	
		if (message.Equals("Next")) {  // Next button
			if (page < (files.Count / fileButtons.Length)) {
				page++;
				ShowBoards(page);
			}
		} else if (message.Equals("Previous")) {  // Previous button
			if (page > 0) {
				page--;
				ShowBoards(page);
			}
		// Image icon buttons
		} else if (message.Equals("Board1")) {
			LoadBoard(files[0 + (page * fileButtons.Length)]);
		} else if (message.Equals("Board2")) {
			LoadBoard(files[1 + (page * fileButtons.Length)]);
		} else if (message.Equals("Board3")) {
			LoadBoard(files[2 + (page * fileButtons.Length)]);
		} else if (message.Equals("Board4")) {
			LoadBoard(files[3 + (page * fileButtons.Length)]);
		} else if (message.Equals("Board5")) {
			LoadBoard(files[4 + (page * fileButtons.Length)]);
		} 
	}
	
	private void LoadBoard(string fileName) {
		
		print (fileName);
		this.gameObject.SetActive(false);
	}
	
	
	// Displays 5 of the boards in the files list
	private void ShowBoards(int page) {
		foreach (GameObject g in fileButtons) {
			g.SetActive(false);
		}
		
		foreach (GameObject g in fileNames) {
			g.SetActive(false);
		}
		
		int count = 0;
		for (int i = page * fileButtons.Length; i < files.Count; i++, count++ ) {
			if (count >= fileButtons.Length) {
				i = files.Count;
				break;
			} else {
				fileButtons[count].SetActive(true);
				fileNames[count].SetActive(true);
				fileButtons[count].SendMessage("EnterText", files[i]);
			}
		}
	}
	
	// Loads files from the Resources folder and store them in the files list
	private void LoadBoardsFromFolder() {
		string[] filePaths = Directory.GetFiles(Application.dataPath + "/Resources/Boards/");
		for (int i = 0; i < filePaths.Length; i++) {
			if (filePaths[i].ToLower().Contains(".txt")) {
				files.Add(filePaths[i].Substring(filePaths[i].LastIndexOf("/")+1));
			}
		}
	}
}
