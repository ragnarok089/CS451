// RadioButton.js
// Used for making a GUITexture function like a radio button
// Very similar to Button.js except that only one can be selected at a time.

// Requires a GUITexture
@script RequireComponent(GUITexture)

var defaultTexture : Texture;
var hoverTexture : Texture;
var clickedTexture : Texture;
var receiver : GameObject;
var message = "";
var otherOptions : GameObject[]; // Contains the other buttons in this radio button group
var selected : boolean;

private var thisGUITexture : GUITexture;

function Start() {
	// Cache the GUITexture component for this GameObject
	thisGUITexture = GetComponent(GUITexture);
	if (selected) {  // If this button is selected
		thisGUITexture.texture = hoverTexture;
		if (receiver) {
			receiver.SendMessage("Message", message);
		}
	}
} 
 
// Updates the texture when the mouse goes over the GUITexture
function OnMouseEnter() {
	thisGUITexture.texture = hoverTexture;
}

// Updates the texture when the mouse leaves the GUITexture
function OnMouseExit() {
	if (!selected) {
		thisGUITexture.texture = defaultTexture;
	}
}
 
// Updates the texture when the texture is clicked
function OnMouseDown() {
	thisGUITexture.texture = clickedTexture;
}

// Updates the texture when the mouse button is released and sends a message to the receiver
function OnMouseUp() {
	ResetButtons(); // Deselects all other buttons in this group
	selected = true;
	thisGUITexture.texture = hoverTexture;

	if (receiver) {
        receiver.SendMessage("Message", message);
	}
}

// Resets all of the buttons in this radio button group to unselected
function ResetButtons() {
	for (var i : int = 0; i < otherOptions.length; i++) {
		otherOptions[i].GetComponent(RadioButton).selected = false;
		otherOptions[i].GetComponent(GUITexture).texture = otherOptions[i].GetComponent(RadioButton).defaultTexture;
	}
}
 