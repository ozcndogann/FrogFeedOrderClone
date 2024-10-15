using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrogPlacement
{
    public int id;  // Id of the frog
    public float rotation;  // Rotation of the frog (0 down, 90 left, 180 up, 270 right)
    public int color;
    public bool spawnMidGame;  // If true, a frog will appear in the middle of the game
    public int respawnAfterIdPop;  // It shows which frog with this id will be formed when the frog with this id disappears.
}

[System.Serializable]
public class Level
{
    public List<FrogPlacement> frogPlacements; // List of all frogs placements
    public int MoveAmount;
    public int MoveAmountInitial;
    public int GridSizeX;
    public int GridSizeY;
    public float squareSize;
}

public class LevelManager : MonoBehaviour
{
    public GameObject frogPrefab;  
    public List<Level> levels; 
    public Material[] materials;
    public GridCreator gridCreator;
    public List<GameObject> frogs = new List<GameObject>();
    private Dictionary<int, FrogPlacement> placementsById = new Dictionary<int, FrogPlacement>();

    void Start()
    {
        PlayerPrefs.SetInt("Level", 0);
        StartCoroutine(PlacingFrogs());
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].MoveAmountInitial = levels[i].MoveAmount;
        }
    }

    void PlaceFrogs()
    {
        if (PlayerPrefs.GetInt("Level") < levels.Count)
        {
            foreach (FrogPlacement placement in levels[PlayerPrefs.GetInt("Level")].frogPlacements)
            {
                // Only Instantiate frogs with respawnOnDestruction false
                if (!placement.spawnMidGame)
                {
                    InstantiateFrog(placement);
                }
                placementsById[placement.id] = placement;
            }
        }
        else
        {
            Debug.LogWarning("Current level exceeds available levels.");
        }
    }

    void InstantiateFrog(FrogPlacement placement)
    {
        // Find the cell with the corresponding ID
        Cell[] cells = FindObjectsOfType<Cell>();

        foreach (Cell cell in cells)
        {
            if (cell.Id == placement.id)
            {
                // Create the frog and assign it as the child of the cell
                cell.color = placement.color;
                Material frogMaterial = new Material(materials[placement.color]);
                frogPrefab.GetComponentInChildren<Renderer>().material = frogMaterial;
                GameObject frog = Instantiate(frogPrefab, cell.transform.position, Quaternion.Euler(0, placement.rotation, 0));
                frogs.Add(frog);
                frog.transform.parent = cell.transform;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < frogs.Count; i++)
        {
            if (frogs[i].GetComponent<Frog>().Eated && frogs[i] != null)
            {
                frogs.Remove(frogs[i]);
                CheckFrogRespawns();
            }
        }
    }

    void CheckFrogRespawns()
    {
        Cell[] cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            if (cell.transform.childCount == 0)
            {
                foreach (KeyValuePair<int, FrogPlacement> entry in placementsById)
                {
                    if (entry.Value.respawnAfterIdPop == cell.Id && entry.Value.spawnMidGame)
                    {
                        InstantiateFrog(entry.Value);
                        placementsById.Remove(entry.Key);
                        break;
                    }
                }
            }
        }
    }

    IEnumerator PlacingFrogs()
    {
        yield return new WaitForSeconds(.1f);
        PlaceFrogs();
    }
    public void SetLevel(int level)
    {
        if (level >= 0 && level < levels.Count)
        {
            PlayerPrefs.SetInt("Level", level);
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                Destroy(cell.gameObject);
            }
            gridCreator.CreateGrid(levels[PlayerPrefs.GetInt("Level")].GridSizeX, levels[PlayerPrefs.GetInt("Level")].GridSizeY);
            StartCoroutine(PlacingFrogs());
        }
        else
        {
            Debug.LogWarning("Invalid level number.");
        }
    }
}