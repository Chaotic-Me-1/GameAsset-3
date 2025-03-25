using UnityEngine;

public class BodyLeaning : MonoBehaviour
{
    [Header("Joint to Rotate")]
    public Transform targetJoint;

    [Header("Lean Settings")]
    public float forwardLeanLimit = 25f;     // Max forward X
    public float backwardLeanLimit = -12f;   // Max back X
    public float leanSpeed = 20f;            // Degrees per second

    private float currentAngle = 0f;

    void Start()
    {
        if (targetJoint == null)
        {
            Debug.LogError("No joint assigned for BodyLeaning!");
        }

        currentAngle = NormalizeAngle(targetJoint.localEulerAngles.x);
    }

    void LateUpdate()  // LateUpdate to override Animator
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.W))
            input = 1f;
        else if (Input.GetKey(KeyCode.S))
            input = -1f;

        // Apply input to change angle
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

