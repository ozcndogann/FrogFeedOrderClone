using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public GameObject squarePrefab;  // Prefab for the squares to be instantiated
    public GameObject plane;  // The plane on which the squares will be placed

    public float squareSize = 1.0f;  // Size of the squares
    LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        CreateGrid(levelManager.levels[PlayerPrefs.GetInt("Level")].GridSizeX, levelManager.levels[PlayerPrefs.GetInt("Level")].GridSizeY);
    }

    public void CreateGrid(int xCount, int yCount)
    {
        // Get the size of the plane
        Vector3 planeSize = plane.GetComponent<Renderer>().bounds.size;

        // Calculate grid size
        float gridWidth = xCount * levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize;
        float gridHeight = yCount * levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize;

        // Get the center of the plane
        Vector3 planeCenter = plane.transform.position;

        // Calculate the bottom left corner of the grid
        Vector3 gridOrigin = new Vector3(
            planeCenter.x - gridWidth / 2.0f + levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize / 2.0f,
            planeCenter.y + 0.01f,  // Slightly elevate the squares above the plane object
            planeCenter.z - gridHeight / 2.0f + levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize / 2.0f
        );

        int idCounter = 0;  // Counter for cell IDs

        // Grid creation
        for (int j = 0; j < yCount; j++)  // Outer loop for the y-axis (rows)
        {
            for (int i = 0; i < xCount; i++)  // Inner loop for the x-axis (columns)
            {
                Vector3 position = new Vector3(
                    gridOrigin.x + i * levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize,
                    gridOrigin.y,
                    gridOrigin.z + j * levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize
                );
                GameObject square = Instantiate(squarePrefab, position, Quaternion.identity);

                // Set the size of the square
                square.transform.localScale = new Vector3(levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize, square.transform.localScale.y, levelManager.levels[PlayerPrefs.GetInt("Level")].squareSize);

                // Add the square as a child of the plane object
                square.transform.parent = plane.transform;

                // Assign ID to the cell
                Cell cell = square.GetComponent<Cell>();
                if (cell != null)
                {
                    cell.Id = idCounter;
                    idCounter++;
                }
            }
        }
    }
}