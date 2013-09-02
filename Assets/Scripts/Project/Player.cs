using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public bool hasWon;
	public Piece[] piece;
	//include array of pieces?
	
	// Use this for initialization
	void Start () {
		hasWon = false;
	}
	
	//Responsible for saving
	public void Save(string path)
	{
	}
	
}
