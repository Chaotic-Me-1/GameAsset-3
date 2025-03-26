using UnityEngine;

public class WindowShutter : MonoBehaviour
{
    [Header("Shutter Setup")]
    public Transform shutterTransform;
    public Transform handleTransform;
    public Transform handTarget;

    [Header("Movement Settings")]
    public float movementSensitivity = 0.5f;
    public float smoothing = 10f;
    public float handStickiness = 20f; // Higher = more stuck feeling

    [Header("Shutter Movement Offset")]
    public Vector3 shutterOffset = new Vector3(0f, -0.543f, 0.125f);

    private bool isTouching = false;
    private bool isGrabbing = false;

    private Vector3 initialPosition;
    private float movePercent = 0f; // 0 = open, 1 = closed

    void Start()
    {
        initialPosition = shutterTransform.position;
    }

    void Update()
    {
        if (isTouching && Input.GetMouseButtonDown(0))
        {
            isGrabbing = true;
            Debug.Log("Started grabbing shutter");
        }

        if (Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
            Debug.Log("Released shutter");
        }

        if (isGrabbing)
        {
            // Smooth movement of shutter based on mouse
            float mouseY = Input.GetAxis("Mouse Y");
            movePercent -= mouseY * movementSensitivity;
            movePercent = Mathf.Clamp01(movePercent);

            // Stick the hand to the handle
            handTarget.position = Vector3.Lerp(handTarget.position, handleTransform.position, Time.deltaTime * handStickiness);
        }

        // Smooth shutter movement
        Vector3 targetPos = initialPosition + (shutterOffset * movePercent);
        shutterTransform.position = Vector3.Lerp(shutterTransform.position, targetPos, Time.deltaTime * smoothing);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            isTouching = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            isTouching = false;
    }
}





