using UnityEngine;

// Script by OlafRT
// Lets us open and close the window shutter by dragging it up/down.
// We're interacting with a handle part of it, not the whole shutter.

public class WindowShutter : MonoBehaviour
{
    [Header("Shutter Setup")]
    public Transform shutterTransform;
    public Transform handleTransform;
    public Transform handTarget;

    [Header("Movement Settings")]
    public float movementSensitivity = 0.5f;
    public float smoothing = 10f;
    public float handStickiness = 20f;

    [Header("Shutter Movement Offset")]
    public Vector3 shutterOffset = new Vector3(0f, -0.543f, 0.125f);

    private bool isTouching = false;
    private bool isGrabbing = false;

    private Vector3 initialPosition;
    private float movePercent = 0f;

    void Start()
    {
        initialPosition = shutterTransform.position;
    }

    void Update()
    {
        if (isTouching && Input.GetMouseButtonDown(0))
        {
            isGrabbing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isGrabbing = false;
        }

        if (isGrabbing)
        {
            // Smooth movement of shutter based on mouse
            float mouseY = Input.GetAxis("Mouse Y");
            movePercent -= mouseY * movementSensitivity;
            movePercent = Mathf.Clamp01(movePercent);

            // Stick hand to the handle
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





