using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColorChanger : MonoBehaviour
{
    Cell cell;
    public Material[] materials;

    void Start()
    {
        cell = gameObject.GetComponent<Cell>();
    }

    
    void Update()
    {
        UpdateCellColors();
    }
    public void UpdateCellColors()
    {
        Cell[] cells = FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            Renderer renderer = cell.GetComponent<Renderer>();
            if (renderer != null && cell.color < materials.Length)
            {
                renderer.material = materials[cell.color];
            }
        }
    }
}
