using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public Texture texture;
	public Shader shader;
	public bool containsPiece = false;
	public Piece piece;

	// Use this for initialization
	void Start () {
		renderer.material = new Material(shader);
		renderer.material.mainTexture = texture;
	}
	
	// Sets the texture of the tile
	public void SetTexture (Texture t) {
		gameObject.renderer.material.mainTexture = t;
		texture = t;
	}

}
