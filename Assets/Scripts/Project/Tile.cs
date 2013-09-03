using UnityEngine;
using System.Collections;
using System.Xml;

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
		print (t.name);
	}
	
	void Update(){
		if (piece == null)
			containsPiece = false;
		else
			containsPiece = true;
	}
	
	public void Save(XmlWriter writer){
		writer.WriteStartElement("Tile");
		
		writer.WriteElementString("Texture", texture.name);
		writer.WriteElementString ("ContainsPiece", containsPiece.ToString());
		
		if(containsPiece)
			piece.Save (writer);
		
		writer.WriteElementString("XPosition", this.gameObject.transform.position.x.ToString());
		writer.WriteElementString("YPosition", this.gameObject.transform.position.y.ToString());
		writer.WriteElementString("ZPosition", this.gameObject.transform.position.z.ToString());
		
		writer.WriteEndElement();
	}
	
	public void Load(ref XmlReader reader){
		
		float tempX = -1;
		float tempY = -1;
		float tempZ = -1;
				
		while(reader.NodeType != XmlNodeType.EndElement){
			
			reader.Read ();
			
			print ("reader name = "+reader.Name);
			
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
			reader.Read ();
			if (reader.Name == "ContainsPiece"){
				while(reader.NodeType!= XmlNodeType.EndElement){
					reader.Read ();
					if (reader.NodeType == XmlNodeType.Text)
						containsPiece = bool.Parse(reader.Value);
				}
				print ("Loading containspiece = " + containsPiece.ToString());
				
				if (containsPiece){
					reader.Read();
					if(reader.Name == "Tile")
						piece.Load(ref reader);
				}
			}
			
			reader.Read ();
			if (reader.Name == "XPosition"){
				while(reader.NodeType!= XmlNodeType.EndElement){
					reader.Read ();
					if (reader.NodeType == XmlNodeType.Text)
						tempX = float.Parse(reader.Value);
				}
					print ("Loading tile xPos");
			}
			
			reader.Read ();
			if (reader.Name == "YPosition"){
				while(reader.NodeType!= XmlNodeType.EndElement){
					reader.Read ();
					if (reader.NodeType == XmlNodeType.Text)
						tempY = float.Parse(reader.Value);
				}
					print ("Loading tile yPos");
			}
			
			reader.Read ();
			if (reader.Name == "ZPosition"){
				while(reader.NodeType!= XmlNodeType.EndElement){
					reader.Read ();
					if (reader.NodeType == XmlNodeType.Text)
						tempZ = float.Parse(reader.Value);
				}
					print ("Loading tile zPos");
			}
		}
		
		print ("Finished loading tile");
		//Don't forget to actually set the new location, as we only stored in temp vars
		//this.gameObject.transform.position.Set(tempX, tempY,tempZ);
	}

}
