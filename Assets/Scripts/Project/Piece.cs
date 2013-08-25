﻿using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	
	public Player player;
	public Texture texture;
	public Shader shader;
	public string direction = "forward";
	public string type = "";
	public bool hasMoved = false;
	public System.Collections.Generic.List<string> moves;
	
	//Directions
	public const string FORWARD = "forward";
	public const string BACKWARD = "backward";
	public const string LEFT = "left";
	public const string RIGHT = "right";
	public const string FORWARDLEFT = "forwardleft";
	public const string FORWARDRIGHT = "forwardright";
	public const string BACKWARDLEFT = "backwardleft";
	public const string BACKWARDRIGHT = "backwardright";
	
	// Use this for initialization
	void Start () {
		//moves = new System.Collections.Generic.List<string>();
		renderer.material = new Material(shader);
		renderer.material.mainTexture = texture;
		SetDirection(direction);
	}
	
	// Sets the texture of the piece
	public void SetTexture (Texture t) {
		gameObject.renderer.material.mainTexture = t;
		texture = t;
	}
	
	public void SetDirection (string d) {
		direction = d;
		
		if (direction == "forward"){
			transform.localEulerAngles =  new Vector3(0,180,0);
		}
		else if (direction == "backward"){
			transform.localEulerAngles =  new Vector3(0,0,0);
		}
		else if (direction == "left"){
			transform.localEulerAngles =  new Vector3(0,90,0);
		}
		else if (direction == "right"){
			transform.localEulerAngles =  new Vector3(0,270,0);
		}
	}
	
	public void SetPiece (Tile t){
		transform.position = t.transform.position;
		t.piece = this;
	}
		
	public void RemovePiece(){
		GameObject.Destroy(this.gameObject);
	}
	
	public void AddMove(string direction) {
		bool alreadyExists = false;
		for (int i = 0; i < moves.Count; i++) {
			if (moves[i].Equals(direction)) {
				alreadyExists = true;
				break;
			}
		}
		if (!alreadyExists) {
			moves.Add(direction);	
		}
	}
	
	public void RemoveMove(string direction) {
		for (int i = 0; i < moves.Count; i++) {
			if (moves[i].Equals(direction)) {
				moves.RemoveAt(i);
				break;
			}
		}
	}
}