using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildGrid : MonoBehaviour {

    public TileModel tilePrefab;
    public int gridWidth = 15, gridHeight = 9;
    public float tilePadding = 0.0F; // Spacing between tiles.
    public int numberOfMines = 20;
    int total_tiles = 0;
    bool upsideDown = false;
	string state = "ingame";

    public static TileModel[] allTiles;
    public static List<TileModel> minedTiles;
    public static List<TileModel> unmindedTiles;

    // Use this for initialization
    void Start () {
        CreateTiles();
	}

    // Displays the triangles for one side
    void CreateTiles ()
    {
        allTiles = new TileModel[gridWidth * gridHeight];

        float xOffset = 0.0F, yOffset = 0.0F;
        float yUpsideDist = 0.2886751F; // (1/3) * cos(30)
        float yTriOffsetDist = 0.8660254037F; // cos(30)

		// Build the grid, first rows and then columns
		for (int yTilesCreated = 0; yTilesCreated < gridHeight; yTilesCreated += 1)
		{
			for (int xTilesCreated = 0; xTilesCreated < gridWidth; xTilesCreated += 1)
			{
				Vector3 position = new Vector3();
				Quaternion rotation = new Quaternion();
				if ((xTilesCreated + yTilesCreated) % 2 == 0)
				{
					position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
					rotation = Quaternion.identity;
					upsideDown = false;
				} else
				{
					position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset + yUpsideDist, transform.position.z);
					rotation = Quaternion.Euler(0, 0, 180);
					upsideDown = true;
				}

				TileModel new_tile = (TileModel)Instantiate(tilePrefab, position, rotation);
				new_tile.ID = total_tiles;
				new_tile.tilesPerRow = gridWidth;
				new_tile.isUpsideDown = upsideDown;
				allTiles[total_tiles] = new_tile;
				total_tiles++;
				xOffset += 0.5F + tilePadding;
			}

			xOffset = 0.0F;
			yOffset += yTriOffsetDist + tilePadding;
		}

        AssignMines();
    }

	void OnGUI()
	{
		GUI.Box(new Rect(5, 5, 100, 50), state);
	}

    void AssignMines()
    {
        unmindedTiles = new List<TileModel>(allTiles);
        minedTiles = new List<TileModel>();
		int numberOfMines = 20;

        for (int assigned = 0; assigned < numberOfMines; ++assigned)
        {
            TileModel currentTile = (TileModel)unmindedTiles[Random.Range(0, unmindedTiles.Count)];

            currentTile.GetComponent<TileModel>().isMined = true;

            minedTiles.Add(currentTile);
            unmindedTiles.Remove(currentTile);
        }
    }
}
