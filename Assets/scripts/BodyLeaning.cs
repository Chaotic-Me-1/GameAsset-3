using UnityEngine;

// Script by OlafRT
// Rotates a specified body joint (like the upper torso) 
// forward or backward depending on whether W or S is pressed. 
// Limits are applied to prevent over-rotation, and LateUpdate 
// is used to override animations. We’re making a plane-seat simulator here!

public class BodyLeaning : MonoBehaviour
{
    [Header("Joint to Rotate")]
    public Transform targetJoint;

    [Header("Lean Settings")]
    public float forwardLeanLimit = 25f;     // Max forward X lean 
    public float backwardLeanLimit = -12f;   // Max backward X lean
    public float leanSpeed = 20f;            // Degrees per second of leaaaaning

    private float currentAngle = 0f;

    void Start()
    {
        if (targetJoint == null)
        {
            Debug.LogError("No joint assigned (BodyLeaning!)");
        }

        currentAngle = NormalizeAngle(targetJoint.localEulerAngles.x);
    }

    void LateUpdate()  // Need to use LateUpdate (to override Animator)
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.W))
            input = 1f;
        else if (Input.GetKey(KeyCode.S))
            input = -1f;

        // Apply input to change the angle
        currentAngle += input * leanSpeed * Time.deltaTime;

        // Clamp to min/max lean
        currentAngle = Mathf.Clamp(currentAngle, backwardLeanLimit, forwardLeanLimit);

        // Apply to joint
        Vector3 newRotation = targetJoint.localEulerAngles;
        newRotation.x = currentAngle;
        targetJoint.localEulerAngles = newRotation;
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}

