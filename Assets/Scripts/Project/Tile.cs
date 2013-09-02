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
	
	public void Save(XmlWriter writer)
	{
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

}
