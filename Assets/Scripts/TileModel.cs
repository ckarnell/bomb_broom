using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour {

    public int ID;
    public TextMesh displayText;
    public int tilesPerRow;
    public bool isMined = false;
    public bool isUpsideDown;

    public Material materialIdle;
    public Material materialMouseOver;

    // Adjacent tiles
    public TileModel bottomRightOuter; // Only on rightside up triangles.
    public TileModel bottomRight;
    public TileModel bottom;
    public TileModel bottomLeft;
    public TileModel bottomLeftOuter; // Only on rightside up triangles.

    public TileModel leftOuter;
    public TileModel left;

    public TileModel topLeftOuter; // Only on upsidedown triangles.
    public TileModel topLeft;
    public TileModel top;
    public TileModel topRight;
    public TileModel topRightOuter; // Only on upsidedown triangles.

    public TileModel rightOuter;
    public TileModel right;


    void OnMouseOver()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = materialMouseOver;
    }

    void OnMouseExit()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = materialIdle;
    }

	void OnMouseDown()
	{
		Debug.Log ("ID: ");
		Debug.Log (ID);
	}

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
