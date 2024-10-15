using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Cell : MonoBehaviour
{
    public int Id { get; set; }
    public int color;
    public GameObject frog; // Is there a frog in this cell 
    public FrogPlacement frogPlacement; // Placement of the frog in this cell

    private void Start()
    {
        gameObject.name += Id;
    }
    private void OnMouseDown()
    {
        Debug.Log("Cell ID: " + Id);
    }

    private void Update()
    {
        // If the cell has no children, set the cell color to the default color
        if (gameObject.transform.childCount == 0)
        {
            color = 5;
        }
    }

    // To display on Gizmos
    private void OnDrawGizmos()
    {
        // Get the world position of the cell
        Vector3 position = transform.position + Vector3.up * 0.5f;

        // Use GUI elements between Handles.BeginGUI and Handles.EndGUI to draw text
        Handles.BeginGUI();

        // Calculate screen position
        var view = SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(position);

        // Create GUI style
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;  // Set text color to red
        style.alignment = TextAnchor.MiddleCenter;

        // Draw text to screen
        Vector2 size = style.CalcSize(new GUIContent("ID: " + Id));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), view.camera.pixelHeight - screenPos.y, size.x, size.y), "ID: " + Id, style);

        Handles.EndGUI();
    }
}