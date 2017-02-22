using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileModel : MonoBehaviour {

    public int ID;
    public TextMesh displayText;
    public int tilesPerRow;
	public int mineCount = 0;
	public string state = "idle";
    public bool isMined = false;
    public bool isUpsideDown;

    public Material materialIdle;
    public Material materialMouseOver;
	public Material materialMouseDown;
	public Material materialFlagged;

    // Adjacent tiles
	public List<TileModel> adjacentTiles = new List<TileModel>();
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


	void assignAdjacentTiles()
	{
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

	void SetFlag() {
		Renderer renderer = GetComponent<Renderer>();
		if (state == "idle") {
			state = "flagged";
			renderer.material = materialFlagged;
		} else if (state == "flagged") {
			state = "idle";
			renderer.material = materialMouseOver;
		}
	}

	void RevealTile() {
		Renderer renderer = GetComponent<Renderer>();
		renderer.material = materialIdle;
		state = "revealed";
	}

    void OnMouseOver()
    {
		Renderer renderer = GetComponent<Renderer>();
		if (state == "idle") {
			if (Input.GetMouseButton(0)) { // True if left click is depressed
				renderer.material = materialMouseDown;
			} else if (Input.GetMouseButtonDown(1)) { // True if right click is clicked
				SetFlag();
			} else if (Input.GetMouseButtonUp(0)) { // True if left click is released
				RevealTile();
			} else {
				renderer.material = materialMouseOver;
			}
		} else if (state == "flagged") {
			if (Input.GetMouseButtonDown(1)) {
				SetFlag();
			}
		}
    }

    void OnMouseExit()
    {
		if (state == "idle") {
	        Renderer renderer = GetComponent<Renderer>();
			renderer.material = materialIdle;
		}
    }

	void OnMouseDown()
	{
//		Renderer renderer = GetComponent<Renderer>();
//		renderer.material = materialMouseDown;
//		Debug.Log ("ID: " + ID.ToString() + "\nMines: " + mineCount.ToString() + " Mined: " + isMined.ToString());
		Debug.Log("State: " + state);
	}

	void OnMouseUpAsButton()
	{
//		state = "revealed";
	}

	// Use this for initialization
	void Start () {
		// Name the tiles for much easier debugging
		gameObject.name = "Tile " + ID.ToString ();

		assignAdjacentTiles();
		// Add the new tiles to the adjacent tile array
		if (bottomRightOuter) {adjacentTiles.Add(bottomRightOuter);}
		if (bottomRight)      {adjacentTiles.Add(bottomRight);}
		if (bottom)           {adjacentTiles.Add(bottom);}
		if (bottomLeft)       {adjacentTiles.Add(bottomLeft);}
		if (bottomLeftOuter)  {adjacentTiles.Add(bottomLeftOuter);}
		if (leftOuter)        {adjacentTiles.Add(leftOuter);}
		if (left)             {adjacentTiles.Add(left);}
		if (topLeftOuter)     {adjacentTiles.Add(topLeftOuter);}
		if (topLeft)          {adjacentTiles.Add(topLeft);}
		if (top)              {adjacentTiles.Add(top);}
		if (topRight)         {adjacentTiles.Add(topRight);}
		if (topRightOuter)    {adjacentTiles.Add(topRightOuter);}
		if (right)            {adjacentTiles.Add(right);}

		// Count the surrounding adjacent mines
		foreach (var adjacentTile in adjacentTiles) {if (adjacentTile.isMined) mineCount++;}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
