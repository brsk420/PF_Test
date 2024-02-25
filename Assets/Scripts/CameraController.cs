using UnityEngine;

public class CameraMovement : MonoBehaviour
{
   
    public float movementSpeed = 5f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 5f;
    public float minDistanceFromGround = 2f;
    public float maxDistanceFromGround = 20f;

    [Header("Camera Distance")]
    [SerializeField] private bool infinity = false;
    public float minDistanceFromPlayer = 0f;
    public float maxDistanceFromPlayer = 20f;
    
    
    private Transform player;
    private float distanceFromGround;
    private Vector3 movementDirection;
    private RaycastHit groundHit;

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit))
        {
            distanceFromGround = Mathf.Clamp(groundHit.distance, minDistanceFromGround, maxDistanceFromGround);
        }
    }

    void LateUpdate()
    {
        HandleCameraRotation();
        HandleCameraMovement();
        HandleCameraZoom();
        AdjustCameraHeight();
        FixDistance();
    }

    void HandleCameraRotation()
    {
        float rotationInput = Input.GetAxis("Camera Rotate") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotationInput, Space.World);
    }

    void HandleCameraMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
    
        // Получаем направление взгляда камеры в плоскости земли
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
    
        // Получаем вектор движения в плоскости земли
        Vector3 movementDirection = (horizontalInput * transform.right + verticalInput * cameraForward).normalized;
    
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
    }

    void HandleCameraZoom() {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        if (distanceFromGround == maxDistanceFromGround && scrollInput < 0f) return;
        if (distanceFromGround == minDistanceFromGround && scrollInput > 0f) return;
        
        float newDistanceFromGround = distanceFromGround - scrollInput * zoomSpeed;
        distanceFromGround = Mathf.Clamp(newDistanceFromGround, minDistanceFromGround, maxDistanceFromGround);
        transform.position += (transform.forward * scrollInput * zoomSpeed);
    }

    void AdjustCameraHeight()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit))
        {
            float newPosY = groundHit.point.y + distanceFromGround;
            transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
        }
    }

    void FixDistance() {
        if (player == null || infinity) return;

        Vector3 directionToTarget = player.position - transform.position;
        directionToTarget.y = 0f; //расстояние до игрока измеряем в горизонтальной плоскости
        float currentDistance = directionToTarget.magnitude;

        if (currentDistance > maxDistanceFromPlayer)
        {
            transform.position += directionToTarget.normalized * (currentDistance - maxDistanceFromPlayer);
        }
        else if (currentDistance < minDistanceFromPlayer)
        {
            transform.position -= directionToTarget.normalized * (minDistanceFromPlayer - currentDistance);
        }
    }
}
