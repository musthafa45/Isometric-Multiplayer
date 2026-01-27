using UnityEngine;

public class MobileCameraController : MonoBehaviour {
    [Header("Camera Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;
    [SerializeField] private float zoomSpeed = 2f;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 1f;
    [SerializeField] private Vector2 minPanBounds = new Vector2(-50f, -50f);
    [SerializeField] private Vector2 maxPanBounds = new Vector2(50f, 50f);
    [SerializeField] private LayerMask groundLayer; // Assign your ground/terrain layer

    [Header("Perspective Settings")]
    [SerializeField] private float cameraHeight = 10f; // Height above ground
    [SerializeField] private float cameraAngle = 45f; // Camera tilt angle

    [Header("State Flags")]
    public bool isPanning = false;
    public bool isZooming = false;

    private Vector3 touchStart;
    private Vector3 cameraStartPos;
    private float initialPinchDistance;
    private float initialCameraDistance;
    private Plane groundPlane;

    void Awake() {
        if (cam == null)
            cam = Camera.main;

        // Create a ground plane at Y=0 for raycasting
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        // Set initial camera angle
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
    }

    void Update() {
        HandleInput();

        // Editor mouse controls for testing
#if UNITY_EDITOR
        HandleMouseInput();
#endif
    }

    void HandleInput() {
        // No touches
        if (Input.touchCount == 0) {
            isPanning = false;
            isZooming = false;
            return;
        }

        // Single touch - Panning
        if (Input.touchCount == 1) {
            HandlePanning();
        }
        // Two touches - Zooming
        else if (Input.touchCount == 2) {
            HandleZooming();
        }
    }

    void HandlePanning() {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began) {
            isPanning = true;
            isZooming = false;

            touchStart = GetWorldPosition(touch.position);
            cameraStartPos = transform.position;
        }
        else if (touch.phase == TouchPhase.Moved && isPanning) {
            Vector3 currentWorldPos = GetWorldPosition(touch.position);

            if (touchStart != Vector3.zero && currentWorldPos != Vector3.zero) {
                Vector3 direction = touchStart - currentWorldPos;
                direction.y = 0; // Keep movement on XZ plane

                Vector3 newPosition = cameraStartPos + direction * panSpeed;

                // Apply bounds on XZ plane
                newPosition.x = Mathf.Clamp(newPosition.x, minPanBounds.x, maxPanBounds.x);
                newPosition.z = Mathf.Clamp(newPosition.z, minPanBounds.y, maxPanBounds.y);

                transform.position = newPosition;
            }
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
            isPanning = false;
        }
    }

    void HandleZooming() {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began) {
            isPanning = false;
            isZooming = true;

            initialPinchDistance = Vector2.Distance(touch0.position, touch1.position);
            initialCameraDistance = transform.position.y;
        }
        else if ((touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) && isZooming) {
            float currentPinchDistance = Vector2.Distance(touch0.position, touch1.position);
            float pinchDelta = (initialPinchDistance - currentPinchDistance) * zoomSpeed * Time.deltaTime;

            float newHeight = transform.position.y + pinchDelta;
            newHeight = Mathf.Clamp(newHeight, minZoom, maxZoom);

            // Adjust position to maintain camera angle
            Vector3 newPos = transform.position;
            float heightDelta = newHeight - transform.position.y;

            // Move camera back/forward based on angle to maintain proper zoom
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            newPos.y = newHeight;
            newPos -= forward * heightDelta * Mathf.Tan(cameraAngle * Mathf.Deg2Rad);

            // Apply bounds
            newPos.x = Mathf.Clamp(newPos.x, minPanBounds.x, maxPanBounds.x);
            newPos.z = Mathf.Clamp(newPos.z, minPanBounds.y, maxPanBounds.y);

            transform.position = newPos;
        }
        else if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended) {
            isZooming = false;

            // If one finger remains, switch to panning
            if (Input.touchCount == 1) {
                isPanning = true;
                Touch remainingTouch = Input.GetTouch(0);
                touchStart = GetWorldPosition(remainingTouch.position);
                cameraStartPos = transform.position;
            }
        }
    }

    // Get world position from screen touch using ground plane raycast
    Vector3 GetWorldPosition(Vector2 screenPosition) {
        Ray ray = cam.ScreenPointToRay(screenPosition);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance)) {
            return ray.GetPoint(rayDistance);
        }

        return Vector3.zero;
    }

    // Editor mouse controls for testing
    void HandleMouseInput() {
        // Right mouse button for panning
        if (Input.GetMouseButtonDown(1)) {
            isPanning = true;
            touchStart = GetWorldPosition(Input.mousePosition);
            cameraStartPos = transform.position;
        }

        if (Input.GetMouseButton(1) && isPanning) {
            Vector3 currentWorldPos = GetWorldPosition(Input.mousePosition);

            if (touchStart != Vector3.zero && currentWorldPos != Vector3.zero) {
                Vector3 direction = touchStart - currentWorldPos;
                direction.y = 0;

                Vector3 newPosition = cameraStartPos + direction * panSpeed;
                newPosition.x = Mathf.Clamp(newPosition.x, minPanBounds.x, maxPanBounds.x);
                newPosition.z = Mathf.Clamp(newPosition.z, minPanBounds.y, maxPanBounds.y);

                transform.position = newPosition;
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            isPanning = false;
        }

        // Mouse scroll for zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            float newHeight = transform.position.y - scroll * zoomSpeed * 10f;
            newHeight = Mathf.Clamp(newHeight, minZoom, maxZoom);

            Vector3 newPos = transform.position;
            float heightDelta = newHeight - transform.position.y;

            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            newPos.y = newHeight;
            newPos -= forward * heightDelta * Mathf.Tan(cameraAngle * Mathf.Deg2Rad);

            newPos.x = Mathf.Clamp(newPos.x, minPanBounds.x, maxPanBounds.x);
            newPos.z = Mathf.Clamp(newPos.z, minPanBounds.y, maxPanBounds.y);

            transform.position = newPos;
        }
    }

    void OnGUI() {
        if (Application.isEditor) {
            GUI.Label(new Rect(10, 10, 200, 20), "isPanning: " + isPanning);
            GUI.Label(new Rect(10, 30, 200, 20), "isZooming: " + isZooming);
            GUI.Label(new Rect(10, 50, 200, 20), "Height: " + transform.position.y.ToString("F2"));
            GUI.Label(new Rect(10, 70, 300, 20), "Position: " + transform.position.ToString("F2"));
            GUI.Label(new Rect(10, 90, 300, 20), "Editor: Right-Click pan, Scroll zoom");
        }
    }

    // Public methods to check states
    public bool IsPanning() => isPanning;
    public bool IsZooming() => isZooming;
    public bool IsInteracting() => isPanning || isZooming;
}