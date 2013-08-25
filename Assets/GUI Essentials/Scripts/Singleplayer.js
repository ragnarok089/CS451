// Singleplayer.js
// Controls the singleplayer menu

// GameObject with the MainMenu.js script attached (Main Camera)
var mainMenuController : GameObject;

// Parses messages that are sent from buttons
function Message(value : String) {
	if (value == "New") {
		NewGame();
	} else if (value == "Load") {
		LoadGame();
	} else if (value == "Back") {
		mainMenuController.GetComponent(MainMenu).MainMenu();
	} else {
		print("ERROR: The message: " + value + " is not recognized.  See the Message function in Singleplayer.js");
	}
}

// Function for starting a new game
function NewGame() {
	// Your code goes here for starting a game
	print("Complete this method in Singleplayer.js");
}

// Function for loading a new game
function LoadGame() {
	// Your code goes here for loading a game
	print("Complete this method in Singleplayer.js");
}