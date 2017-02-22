using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BuildGrid : MonoBehaviour {

    public TileModel tilePrefab;
    public static int gridWidth = 15, gridHeight = 9;
	public static int numberOfMines = 20;
    public float tilePadding = 0.0F; // Spacing between tiles.
	public int total_tiles = 0;
	public static int revealedTiles = 0;
	public static int tilesToReveal = (gridWidth * gridHeight) - numberOfMines;
	public static int minesRemaining = numberOfMines;
    public bool upsideDown = false;
	public static string state = "ingame";

    public static TileModel[] allTiles;
    public static List<TileModel> minedTiles;
    public static List<TileModel> unminedTiles;

    // Use this for initialization
    void Start () {
        CreateTiles();
		AssignMines();
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
    }

	void OnGUI()
	{
		if (state == "ingame") {
			GUI.Box(new Rect(5, 5, 100, 50), "Mines Left: " + minesRemaining);
		} else if (state == "gamewon") {
			GUI.Box(new Rect(10,10,200,50), "You win");
			if (GUI.Button(new Rect(10,70,200,50), "Restart")) {Restart();}
		} else if (state == "gameover") {
			GUI.Box(new Rect(10,10,200,50), "You lose");
			if(GUI.Button(new Rect(10,70,200,50), "Restart")) {Restart();}
		}
	}

	void Restart()
	{
		state = "loading";
		revealedTiles = 0;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    void AssignMines()
    {
        unminedTiles = new List<TileModel>(allTiles);
        minedTiles = new List<TileModel>();

        for (int assigned = 0; assigned < numberOfMines; ++assigned)
        {
            TileModel currentTile = (TileModel)unminedTiles[Random.Range(0, unminedTiles.Count)];

            currentTile.GetComponent<TileModel>().isMined = true;

            minedTiles.Add(currentTile);
            unminedTiles.Remove(currentTile);
        }
    }
}
