using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileModel : MonoBehaviour {

    public int ID;
    public TextMesh displayText;
    public int tilesPerRow;
	public int mineCount = 0;
	public int surroundingFlagCount = 0;
	public string state = "idle";
    public bool isMined = false;
    public bool isUpsideDown;

    public Material materialIdle;
    public Material materialMouseOver;
	public Material materialMouseDown;
	public Material materialFlagged;
	public Material materialRevealed;
	public Material materialExploded;

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

	void CountMines() {
		foreach (var adjacentTile in adjacentTiles) {if (adjacentTile.isMined) mineCount++;}
	}

	void OrientText() {
		if (isUpsideDown) {
			Vector3 rotate = new Vector3(0, 0, 180);
			displayText.transform.Rotate(rotate);
			displayText.transform.position = displayText.transform.position - new Vector3(0.0F, 0.05F, 0.0F);
		}
	}

	void ColorText() {
		switch(mineCount)
		{
		case 2: displayText.color = new Color(0, 1, 0, 1); break; // Green
		case 3: displayText.color = new Color(1, 0, 0, 1); break; // Red
		case 4: displayText.color = new Color(1, 0, 1, 1); break; // Purple
		case 5: displayText.color = new Color(90, 8, 27, 1); break; // Brown
		case 6: displayText.color = new Color(0, 161, 193, 1); break; // Teal
		case 7: displayText.color = new Color(1, 1, 1, 1); break; // Black
		case 8: displayText.color = new Color(0.5F, 0.5F, 0.5F, 1.0F); break; // Grey
		}
	}

	void SetFlag() {
		Renderer renderer = GetComponent<Renderer>();
		if (state == "idle") {
			state = "flagged";
			renderer.material = materialFlagged;
			BuildGrid.minesRemaining--;
			foreach (var tile in adjacentTiles) {
				tile.surroundingFlagCount++;
			}
		} else if (state == "flagged") {
			state = "idle";
			renderer.material = materialMouseOver;
			BuildGrid.minesRemaining++;
			foreach (var tile in adjacentTiles) {
				tile.surroundingFlagCount--;
			}
		}
	}

	void RevealTile()
	{
		Renderer renderer = GetComponent<Renderer>();
		if (state == "idle") {
			if (!isMined) {
				state = "revealed";
				renderer.material = materialRevealed;
				CheckWinState();
				if (mineCount != 0) {
					displayText.text = mineCount.ToString();
				} else {
					// Recursively reveal nearby tiles if mineCount is 0
					foreach(var adjacentTile in adjacentTiles) {
						if (adjacentTile.state == "idle") {adjacentTile.RevealTile();}
					}
				}
			} else {
				Explode();
			}
		}
	}

	void CheckWinState()
	{
		BuildGrid.revealedTiles++;
		if (BuildGrid.revealedTiles == BuildGrid.tilesToReveal) {
			BuildGrid.state = "gamewon";
		}
	}

	void Explode()
	{
		foreach (var tile in BuildGrid.allTiles) {
			if (tile.state == "idle") {
				tile.state = "revealed";
				if (tile.isMined) {
					Renderer renderer = tile.GetComponent<Renderer>();
					renderer.material = materialExploded;
				}
			}
		}
		BuildGrid.state = "gameover";
	}

	void RevealSurrounding()
	{
		if (surroundingFlagCount == mineCount) {
			foreach (var tile in adjacentTiles) {
				if (tile.state == "idle") {tile.RevealTile();}
			}
		}
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
		} else if (state == "revealed") {
			if (Input.GetMouseButtonUp(0)) {
				if (Input.GetMouseButton(1)) {
					RevealSurrounding();
				}
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
//		Debug.Log("State: " + state);
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
		if (rightOuter)		  {adjacentTiles.Add(rightOuter);}
		if (right)            {adjacentTiles.Add(right);}

		// Count the surrounding adjacent mines
		CountMines();

		OrientText();
		ColorText();

	}
	
	// Update is called once per frame
	void Update () {

	}
}
