using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float zoomAmount = 18f; 
    public float minZoom = 30f;
    public float maxZoom = 540f;
    public float panSpeed = 50f;

    private GridManager gridManager;
    private Vector3 dragOrigin;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        cam = GetComponent<Camera>();
        gridManager = GridManager.Instance;
        cam.orthographic = true;
        CalculateCameraBounds();
    }

    private void Update()
    {
        if (gridManager != null && gridManager.isRunning)
        {
            MoveCamera();
            ClampCameraPosition();
        }
    }

    private void MoveCamera()
    {
        // Handle mouse drag panning
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }

        // Handle zoom with scroll wheel
        HandleZoom();
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput == 0) return;

        // Store original zoom and mouse position
        float originalSize = cam.orthographicSize;
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = cam.nearClipPlane;
        Vector3 mouseWorldPosBefore = cam.ScreenToWorldPoint(mouseScreenPos);

        // Apply zoom
        float zoomChange = -scrollInput * 160f;
        cam.orthographicSize = Mathf.Clamp(originalSize + zoomChange, minZoom, maxZoom);

        // Get mouse position after zoom
        Vector3 mouseWorldPosAfter = cam.ScreenToWorldPoint(mouseScreenPos);

        // Adjust camera position to maintain mouse position
        cam.transform.position += mouseWorldPosBefore - mouseWorldPosAfter;

        // Update bounds and clamp
        CalculateCameraBounds();
        ClampCameraPosition();
    }

    private void CalculateCameraBounds()
    {
        float gridWidthTotal = gridManager.gridWidth * gridManager.cellSize;
        float gridHeightTotal = gridManager.gridHeight * gridManager.cellSize;

        float cameraHalfHeight = cam.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * cam.aspect;

        minX = cameraHalfWidth;
        maxX = gridWidthTotal - cameraHalfWidth;
        minY = cameraHalfHeight;
        maxY = gridHeightTotal - cameraHalfHeight;

        if (minX > maxX) minX = maxX = gridWidthTotal / 2;
        if (minY > maxY) minY = maxY = gridHeightTotal / 2;
    }

    private void ClampCameraPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }
}