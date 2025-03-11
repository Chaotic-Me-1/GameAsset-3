using UnityEngine;

public class SunRotation : MonoBehaviour
{
    // Rotation speed in degrees per second.
    public float rotationSpeed = 1.0f;
    
    // Current X rotation.
    private float currentX;
    
    // Store the initial Y and Z rotations.
    private float initialY;
    private float initialZ;

    void Start()
    {
        // Use localEulerAngles in case this object is part of a hierarchy.
        currentX = transform.localEulerAngles.x;
        initialY = transform.localEulerAngles.y;
        initialZ = transform.localEulerAngles.z;
    }

    void Update()
    {
        // Increase the X rotation.
        currentX += rotationSpeed * Time.deltaTime;
        
        // Wrap the angle when a full rotation is reached.
        if (currentX >= 360f)
        {
            currentX -= 360f;
        }

        // Apply the rotation while keeping Y and Z constant.
        transform.localEulerAngles = new Vector3(currentX, initialY, initialZ);
    }
}
