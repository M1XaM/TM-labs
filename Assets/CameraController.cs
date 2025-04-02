using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    public float zoomAmount = 18f; 
    public float minZoom = 30f;
    public float maxZoom = 540f;
    public float panSpeed = 200f;  // Panning (dragging) sensitivity

    private GridManager gridManager;  // Reference to GridManager

    

    private void Start ()
    {
        cam = this.GetComponent<Camera>();
        gridManager = GridManager.Instance;
        cam.orthographic = true;
    }

    private void Update(){
        if (gridManager != null && gridManager.isRunning) // Only allow zoom when the game is running
        {
            MoveCamera();
        }
    }
   
    private void MoveCamera ()
    {

     if (Input.GetMouseButtonDown(0) && cam.orthographicSize <= 540) // Left click to zoom out
        {
            ZoomToPoint(zoomAmount);
        }
    if (Input.GetMouseButtonDown(1) && cam.orthographicSize >= 30) // Left click to zoom in
        {
            ZoomToPoint(-zoomAmount);
        }


    float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float zoomChange = -scrollInput * 160f; // Negative to match trackpad behavior
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + zoomChange, minZoom, maxZoom);

        }
    
    Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            move.y += panSpeed ; // Move camera up
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move.y -= panSpeed ; // Move camera down
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move.x -= panSpeed; // Move camera left
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move.x += panSpeed; // Move camera right
        }

        // Apply movement to camera position
        cam.transform.position += move*40;
    
    }


void ZoomToPoint(float zoomAmount)
    {

       Vector3 click = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);
        // Get mouse position in world space
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(click);

         // Apply incremental zoom
        cam.orthographicSize += zoomAmount ;

        // Calculate direction to move camera
        Vector3 direction = (mouseWorldPos - cam.transform.position) * 0.2f;

        // Move camera slightly towards the zoom point
        cam.transform.position += direction;
    }

}