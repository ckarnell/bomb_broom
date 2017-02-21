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
	public Material materialMouseDown;

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

	bool isInBounds(TileModel[] array, int id)
	{
		if(id >= array.Length || id < 0)
			return false;
		return true;
	}

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
		Renderer renderer = GetComponent<Renderer>();
		renderer.material = materialMouseDown;
		Debug.Log (ID);
	}

	// Use this for initialization
	void Start () {
		// Name the tiles for much easier debugging
		gameObject.name = "Tile " + ID.ToString ();

		// Assign bordering tiles
		TileModel[] tiles = BuildGrid.allTiles;
		if (isInBounds (tiles, ID - tilesPerRow)) {bottom = tiles [ID - tilesPerRow];}
		if(isInBounds(tiles, ID + tilesPerRow)) {top = tiles[ID + tilesPerRow];}

		// Assign upper, outter, and bottom left adjacent tiles
		if (ID % tilesPerRow != 0) {
			if (isInBounds (tiles, ID - tilesPerRow - 1)) {bottomLeft = tiles [ID - tilesPerRow - 1];}
			if (isInBounds (tiles, ID - 1)) {left = tiles [ID - 1];}
			if (isInBounds(tiles, ID + tilesPerRow - 1)) {topLeft = tiles[ID + tilesPerRow - 1];}

			if ((ID - 1) % tilesPerRow != 0) {
				if (isInBounds(tiles, ID - 2)) {leftOuter = tiles [ID - 2];}
				if (isUpsideDown) {
					if (isInBounds (tiles, ID + tilesPerRow - 2)) {topLeftOuter = tiles [ID + tilesPerRow - 2];}
				} else {
					if (isInBounds(tiles, ID - tilesPerRow - 2)) {bottomLeftOuter = tiles[ID - tilesPerRow - 2];}
				}
			}

		}

		// Assign upper, outter, and bottom right adjacent tiles
		if (ID % tilesPerRow != tilesPerRow - 1) {
			if (isInBounds (tiles, ID - tilesPerRow + 1)) {bottomRight = tiles [ID - tilesPerRow + 1];}
			if(isInBounds(tiles, ID + tilesPerRow + 1)) {topRight = tiles[ID + tilesPerRow + 1];}
			if(isInBounds(tiles, ID + 1) && ((ID+1) % tilesPerRow != 0)) {right = tiles [ID + 1];}

			if ((ID + 1) % tilesPerRow != tilesPerRow - 1) {
				if(isInBounds(tiles, ID + 2) && ((ID+2) % tilesPerRow != 0)) {rightOuter = tiles [ID + 2];}
				if (isUpsideDown) {
					if (isInBounds(tiles, ID + tilesPerRow + 2)) {topRightOuter = tiles[ID + tilesPerRow + 2];}
				} else {
					if (isInBounds(tiles, ID - tilesPerRow + 2)) {bottomRightOuter = tiles[ID - tilesPerRow + 2];}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
