using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	
	public void Message(string message) {
		if (message.Equals("Edit")) {
			Application.LoadLevel("Edit");
		} else if (message.Equals("Play")) {
			Application.LoadLevel("Play");
		}
	}	
}
