// CheckBox.js
// Used for making a GUITexture function like a check box

// Requires a GUITexture
@script RequireComponent(GUITexture)

var defaultTexture : Texture;
var hoverTexture : Texture;
var checkedTexture : Texture;
var hoverCheckedTexture : Texture;
var receiver : GameObject;
var checkedMessage = "";
var uncheckedMessage = "";
var selected : boolean;

private var thisGUITexture : GUITexture;

function Start() {
	// Cache the GUITexture component for this GameObject
	thisGUITexture = GetComponent(GUITexture);
	if (selected) {  // If this button is selected
		thisGUITexture.texture = checkedTexture;
	}
} 
 
// Updates the texture when the mouse goes over the GUITexture
function OnMouseEnter() {
	if (selected)
		thisGUITexture.texture = hoverCheckedTexture;
	else
		thisGUITexture.texture = hoverTexture;
}

// Updates the texture when the mouse leaves the GUITexture
function OnMouseExit() {
	if (selected) 
		thisGUITexture.texture = checkedTexture;
	else
		thisGUITexture.texture = defaultTexture;
}
 
// Updates the texture when the texture is clicked
function OnMouseDown() {
	if (selected) 
		thisGUITexture.texture = defaultTexture;
	else
		thisGUITexture.texture = checkedTexture;
}

// Updates the texture when the mouse button is released and sends a message to the receiver
function OnMouseUp() {
	selected = !selected;
	if (selected)
		thisGUITexture.texture = hoverCheckedTexture;
	else
		thisGUITexture.texture = hoverTexture;
		
	if (receiver) {
		if (selected)
			receiver.SendMessage("Message", checkedMessage);
		else
			receiver.SendMessage("Message", uncheckedMessage);
	}
}

function Select(select : boolean) {
	selected = select;
	if (selected)
		thisGUITexture.texture = checkedTexture;
	else
		thisGUITexture.texture = defaultTexture;
	
}