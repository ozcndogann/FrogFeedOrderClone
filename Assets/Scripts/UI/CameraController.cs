using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject plane;    // Plane object

    void Start()
    {
        AdjustPlaneSize();
    }

    void AdjustPlaneSize()
    {
        // Get the reference to the camera
        Camera mainCamera = GetComponent<Camera>();

        if (mainCamera == null)
        {
            Debug.LogError("Camera component is not found");
            return;
        }

        // Get the visible half-height of the camera
        float cameraHeight = mainCamera.orthographicSize * 2;

        // Calculate the visible half-width of the camera (based on aspect ratio)
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Set the plane's size to match the camera's visible dimensions
        plane.transform.localScale = new Vector3(cameraWidth / 10, 1, cameraHeight / 10);

        // Align the plane to the center of the camera
        Vector3 planeCenter = mainCamera.transform.position;
        planeCenter.y = mainCamera.transform.position.y - 1; // Offset the plane slightly below the y-axis
        plane.transform.position = planeCenter;
    }
}