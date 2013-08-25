// Button.js
// Used for making a GUITexture function like a button

// Requires a GUITexture
@script RequireComponent(GUITexture)

var defaultTexture : Texture;
var hoverTexture : Texture;
var clickedTexture : Texture;
var receiver : GameObject;
var message = "";

private var thisGUITexture : GUITexture;
private var mouseOver : boolean = false;

function Start() {
	// Cache the GUITexture component for this GameObject
	thisGUITexture = GetComponent(GUITexture); 
} 
 
// Updates the texture when the mouse goes over the GUITexture
function OnMouseEnter() {
	if (hoverTexture != null) {
		thisGUITexture.texture = hoverTexture;
	}
	mouseOver = true;
}

// Updates the texture when the mouse leaves the GUITexture
function OnMouseExit() {
	if (defaultTexture != null) {
		thisGUITexture.texture = defaultTexture;
	}
	mouseOver = false;
}
 
// Updates the texture when the texture is clicked
function OnMouseDown() {
	if (clickedTexture != null) {
		thisGUITexture.texture = clickedTexture;
	}
}

// Updates the texture when the mouse button is released and sends a message to the receiver
function OnMouseUp() {
	if (mouseOver) {
		if (hoverTexture != null) {
			thisGUITexture.texture = hoverTexture;
		}
    } else {
    	if (defaultTexture != null) {
			thisGUITexture.texture = defaultTexture;
		}
	}
	 
	if (receiver) {
        receiver.SendMessage("Message", message);  // Calls the Message function in the receiver
	}
}