using UnityEngine;
using System.Collections;

public class MouseSelectionEdit : MonoBehaviour {
	
	public TileMenu tileMenu;
	public PieceMenu pieceMenu;
	public GameObject selectedLight;
	private GUILayer gui;
	private Vector3 mousePos;
	private Vector3 worldPos;
	
	
	// Use this for initialization
	void Start () {
		gui = Camera.main.GetComponent<GUILayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonDown(0) && gui.HitTest(Input.mousePosition) == null) {
			mousePos = Input.mousePosition;
			worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, this.transform.position.y));
			selectedLight.transform.position = new Vector3(Mathf.Round(worldPos.x), selectedLight.transform.position.y, Mathf.Round(worldPos.z));
			tileMenu.xPos.text = Mathf.Round(worldPos.x).ToString();
			tileMenu.yPos.text = Mathf.Round(worldPos.z).ToString();
			tileMenu.Message("Select");
			pieceMenu.xPos.text = Mathf.Round(worldPos.x).ToString();
			pieceMenu.yPos.text = Mathf.Round(worldPos.z).ToString();
			pieceMenu.Message("Select");
			
		}
	
	}
}
