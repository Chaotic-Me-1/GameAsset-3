using UnityEngine;

// Script by OlafRT
// Sloooooowly rotates the directional light (sun), this also has a starry dome attached to it, so the stars will also rotate with the sun.
// This script turned out to be a bit redundant as you won't really ever stay in a loop long enough that you get to see a full day/night cycle.

public class SunRotation : MonoBehaviour
{
    // Rotation speed (degrees per second)
    public float rotationSpeed = 1.0f;
    
    private float currentX;
    private float initialY;
    private float initialZ;

    void Start()
    {
        currentX = transform.localEulerAngles.x;
        initialY = transform.localEulerAngles.y;
        initialZ = transform.localEulerAngles.z;
    }

    void Update()
    {
        // Increase X rotation.
        currentX += rotationSpeed * Time.deltaTime;
        
        // Wrap the angle when a full rotation is reached, so we dont get high numbers.
        if (currentX >= 360f)
        {
            currentX -= 360f;
        }

        // Apply the rotation while keeping Y and Z constant.
        transform.localEulerAngles = new Vector3(currentX, initialY, initialZ);
    }
}
