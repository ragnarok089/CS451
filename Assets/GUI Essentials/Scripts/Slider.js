// Slider.js
// Controls a slider made out of two GUITextures

var sliderBar : GUITexture;
var sliderButton : GUITexture;
var sliderValue : GUIText;
var minValue : float = 0.0;
var maxValue : float = 100.0;
var currentValue : float;
var direction : directions = directions.horizontal;

public enum directions {
	horizontal,
	vertical
}

private var mousePos : Vector3;
private var originalX : float;
private var originalY : float;
private var xMin : float;
private var xMax : float;
private var yMin : float;
private var yMax : float;

function Start() {
	yield WaitForEndOfFrame; // The yield command is to make sure the AutoScale script has had a chance to take effect
	originalX = sliderButton.transform.position.x;
	originalY = sliderButton.transform.position.y;
}

// When the mouse is clicked over this GUI element 
function OnMouseDown() {
	mousePos = Input.mousePosition;
	switch (direction) {
		case directions.horizontal: 
			sliderButton.pixelInset.x = 0;
			xMin = Camera.main.ScreenToViewportPoint(Vector3(sliderBar.GetScreenRect().xMin - (sliderButton.GetScreenRect().width / 2), 0, 0)).x;
			xMax = Camera.main.ScreenToViewportPoint(Vector3(sliderBar.GetScreenRect().xMax - (sliderButton.GetScreenRect().width / 2), 0, 0)).x;
			break;
		case directions.vertical: 
			sliderButton.pixelInset.y = 0;
			yMin = Camera.main.ScreenToViewportPoint(Vector3(0, sliderBar.GetScreenRect().yMin - (sliderButton.GetScreenRect().height / 2), 0)).y;
			yMax = Camera.main.ScreenToViewportPoint(Vector3(0, sliderBar.GetScreenRect().yMax - (sliderButton.GetScreenRect().height / 2), 0)).y;
			break;
	}
}

// Moves the GUI element while the mouse is being dragged
function OnMouseDrag() {
	var delta : Vector3 = Input.mousePosition - mousePos;
	delta = Camera.main.ScreenToViewportPoint(delta);
	transform.position += delta;
	var position : Vector3 = transform.position;
	// Limits the position so it is between the ends of the slider bar
	switch (direction) {
		case directions.horizontal:  // For horizontal sliders
			position.x = Mathf.Clamp(position.x, xMin, xMax);
			position.y = originalY;
			break;
		case directions.vertical:  // For vertical sliders
			position.y = Mathf.Clamp(position.y, yMin, yMax);
			position.x = originalX;
			break;
	}
	// Update position
	transform.position = position;
	mousePos = Input.mousePosition;
	
	// Sets the current value of the slider
	var sliderRect : Rect = sliderBar.GetScreenRect();
	var sliderPos : Vector3 = Camera.main.ViewportToScreenPoint(sliderButton.transform.position);
	var rawVal : float; // The value of where the sliderButton is on the slider bar.  Between 0 and sliderBar.xMax
	var length : float; // The length of the sliderBar
	switch (direction) {
		case directions.horizontal:  // For horizontal sliders
			length = sliderRect.xMax - sliderRect.xMin;
			rawVal = sliderButton.GetScreenRect().center.x  - sliderRect.xMin;
			currentValue = (rawVal / length) * (maxValue - minValue);
			break;
		case directions.vertical:  // For vertical sliders
			length = sliderRect.yMax - sliderRect.yMin;
			rawVal = sliderButton.GetScreenRect().center.y  - sliderRect.yMin;
			currentValue = (rawVal / length) * (maxValue - minValue);
			break;
	}
	// Display the currentValue on a GUIText
	if (sliderValue != null) {
		sliderValue.text = Mathf.Round(currentValue).ToString();
	}
}