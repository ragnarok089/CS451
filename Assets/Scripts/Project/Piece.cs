using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;

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
	
	//Responsible for saving
	public void Save(XmlWriter writer)
	{
		writer.WriteStartElement("Piece");
		
		writer.WriteElementString("Texture", texture.name);
		writer.WriteElementString("Direction", direction);
		writer.WriteElementString("Type", type);
		
		writer.WriteStartElement("Moves");
		foreach (string s in moves){
			writer.WriteElementString("Move", s);
		}
		
		writer.WriteElementString("XPosition", this.gameObject.transform.position.x.ToString());
		writer.WriteElementString("YPosition", this.gameObject.transform.position.y.ToString());
		writer.WriteElementString("ZPosition", this.gameObject.transform.position.z.ToString());
		
		writer.WriteEndElement();
				
	}
	
	public void Load(ref XmlReader reader){
		print ("Loading Piece");
		
		float tempX = -1;
		float tempY = -1;
		float tempZ = -1;
		
		print ("Loading Piece");
		
		while(reader.NodeType != XmlNodeType.EndElement){
			
			reader.Read ();
			
			if (reader.Name == "Texture"){
				while(reader.NodeType != XmlNodeType.EndElement){
					reader.Read ();
					if(reader.NodeType == XmlNodeType.Text){
						//texture.name = reader.Value;
					}
				}
				//texture.name = reader.Value;
				print ("Loading texture");
			}
			
			reader.Read();
			if (reader.Name == "Direction"){
				while(reader.NodeType != XmlNodeType.EndElement){
					reader.Read ();
					if(reader.NodeType == XmlNodeType.Text){
						direction = reader.Value;
					}
				}
				print ("Loading direction");
			}
			
			reader.Read();
			if (reader.Name == "Type"){
				while(reader.NodeType != XmlNodeType.EndElement){
					reader.Read ();
					if(reader.NodeType == XmlNodeType.Text){
						type = reader.Value;
					}
				}
				print ("Loading type");
			}
			
			reader.Read();
			if(reader.Name == "Moves"){
				while(reader.NodeType != XmlNodeType.EndElement)
				{
					reader.Read();
					
				}
			}
		}
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
