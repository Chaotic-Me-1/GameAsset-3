using UnityEngine;

public class ArmTargetController : MonoBehaviour
{
    public Transform armTarget;
    public Transform shoulder;
    public Transform cameraTransform; // Main camera reference

    public float moveSpeed = 0.2f;
    public float depthSpeed = 0.5f;
    public float maxDistance = 0.6f;

    private bool isControlling = false;
    private Vector3 defaultOffset;

    void Start()
    {
        // Store initial offset from shoulder in world space
        defaultOffset = armTarget.position - shoulder.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            isControlling = true;

        if (Input.GetMouseButtonUp(1))
            isControlling = false;

        if (isControlling)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            // Move in camera-relative space
            Vector3 rightMovement = cameraTransform.right * mouseX;
            Vector3 upMovement = cameraTransform.up * mouseY;
            Vector3 move = (rightMovement + upMovement) * moveSpeed;

            // Move forward/back with scroll wheel (depth control)
            Vector3 forwardMovement = cameraTransform.forward * scroll * depthSpeed;

            // New target position
            Vector3 newPos = armTarget.position + move + forwardMovement;

            // Clamp distance from shoulder
            float distance = Vector3.Distance(newPos, shoulder.position);
            if (distance > maxDistance)
            {
                Vector3 direction = (newPos - shoulder.position).normalized;
                newPos = shoulder.position + direction * maxDistance;
            }

            // Apply position
            armTarget.position = newPos;
        }
    }

    void LateUpdate() // Make sure it runs after movement
    {
        if (isControlling)
        {
            Vector3 directionFromShoulder = armTarget.position - shoulder.position;
            if (directionFromShoulder != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionFromShoulder);
                armTarget.rotation = Quaternion.Slerp(armTarget.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }
}

