// MainMenu.js
// Controls the various screens of the main menu

// List of GUI elements
var mainMenuElements : GameObject;
var singleplayerElements : GameObject;
var multiplayerElements : GameObject;
var optionsElements : GameObject;

// This function is called by the buttons who set this GameObject as their receiver
function Message(value : String) {
	if (value == "Main Menu") {
		MainMenu();
	} else if (value == "Singleplayer") {
		Singleplayer();
	} else if (value == "Multiplayer") {
		Multiplayer();
	} else if (value == "Options") {
		Options();
	} else if (value == "Quit") {
		Quit();
	} else {
		print("ERROR: The message: " + value + " is not recognized.  See the Message function in MainMenu.js");
	}
}

function MainMenu() {
	// Disable everything
	Disable();
	// Enable the elements we need
	mainMenuElements.SetActive(true);
}

function Singleplayer() {
	// Disable everything
	Disable();
	// Enable the elements we need
	singleplayerElements.SetActive(true);
}

function Multiplayer() {
	// Disable everything
	Disable();
	// Enable the elements we need
	multiplayerElements.SetActive(true);
}

function Options() {
	// Disable everything
	Disable();
	// Enable the elements we need
	optionsElements.SetActive(true);
}

function Quit() {
	// Quit the game.  This will not work in WebPlayer builds
	Application.Quit();
}

// Hides every GUI element that is listed above
function Disable() {
	mainMenuElements.SetActive(false);
	singleplayerElements.SetActive(false);
	multiplayerElements.SetActive(false);
	optionsElements.SetActive(false);
}