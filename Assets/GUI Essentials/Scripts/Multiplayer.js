// Multiplayer.js
// Controls the multiplayer menu

// GameObject with the MainMenu.js script attached (Main Camera)
var mainMenuController : GameObject;

// Parses messages that are sent from buttons
function Message(value : String) {
	if (value == "Join") {
		JoinGame();
	} else if (value == "Host") {
		HostGame();
	} else if (value == "Back") {
		mainMenuController.GetComponent(MainMenu).MainMenu();
	} else {
		print("ERROR: The message: " + value + " is not recognized.  See the Message function in Multiplayer.js");
	}
}

// Function for joining a multiplayer game
function JoinGame() {
	// Your code goes here for joining a game
	print("Complete this method in Multiplayer.js");
}

// Function for hosting a multiplayer game
function HostGame() {
	// Your code goes here for hosting a game
	print("Complete this method in Multiplayer.js");
}