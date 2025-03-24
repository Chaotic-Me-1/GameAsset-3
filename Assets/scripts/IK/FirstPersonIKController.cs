using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonIKController : MonoBehaviour
{
    [Header("Arm IK Settings")]
    // Target for your arm IK system (should be assigned in the Inspector)
    public Transform armIKTarget;      
    // The base position for the arm (typically the right shoulder)
    public Transform rightShoulder;    
    // How far to the right the arm IK target should be (so it sits naturally at the right side)
    public float armLateralOffset = 0.2f; 
    // Default distance from the shoulder to the IK target
    public float defaultArmDistance = 0.5f; 
    // How far the arm can retract or extend
    public float minArmDistance = 0.3f;  
    public float maxArmDistance = 1.0f;  
    // Speed at which the arm extends/retracts when moving the mouse vertically
    public float armExtensionSpeed = 0.5f; 

    // Current computed arm distance
    private float currentArmDistance;

    [Header("Spine & Neck Settings")]
    // Joint to rotate for the spine (e.g., upper torso)
    public Transform spineJoint; 
    // Joint to rotate for the neck (to adjust head/neck orientation)
    public Transform neckJoint;  
    // Sensitivity multipliers for rotation offsets
    public float spineRotationSensitivity = 5f;
    public float neckRotationSensitivity = 5f;
    // Maximum allowed rotation (in degrees) for spine and neck
    public float maxSpineRotation = 20f; 
    public float maxNeckRotation = 15f;  

    // Default rotations for spine and neck (captured on Start)
    private Quaternion defaultSpineRotation;
    private Quaternion defaultNeckRotation;

    void Start()
    {
        currentArmDistance = defaultArmDistance;

        if (spineJoint)
            defaultSpineRotation = spineJoint.localRotation;
        if (neckJoint)
            defaultNeckRotation = neckJoint.localRotation;
    }

    void Update()
    {
        // If CTRL is held, modify the IK target and joint rotations
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // ----- Arm IK Adjustment -----
            // Read vertical mouse movement (Y axis)
            float mouseY = Input.GetAxis("Mouse Y");
            // Subtract mouseY so that moving the mouse upward extends the arm forward.
            // (Invert the sign if needed for your setup.)
            currentArmDistance -= mouseY * armExtensionSpeed * Time.deltaTime;
            // Clamp the extension to avoid unnatural stretching
            currentArmDistance = Mathf.Clamp(currentArmDistance, minArmDistance, maxArmDistance);

            // Calculate the new target position based on the shoulder position,
            // the camera's forward direction, and a lateral offset (to the right)
            Vector3 targetPosition = rightShoulder.position 
                + (transform.forward * currentArmDistance) 
                + (transform.right * armLateralOffset);
            if (armIKTarget)
            {
                armIKTarget.position = targetPosition;
            }

            // ----- Spine & Neck Adjustment -----
            // Read horizontal mouse movement (X axis)
            float mouseX = Input.GetAxis("Mouse X");
            // Calculate rotation offsets (clamped so they don't over-rotate)
            float spineOffset = Mathf.Clamp(mouseX * spineRotationSensitivity, -maxSpineRotation, maxSpineRotation);
            float neckOffset = Mathf.Clamp(mouseX * neckRotationSensitivity, -maxNeckRotation, maxNeckRotation);

            if (spineJoint)
            {
                // Rotate the spine joint around its local Y axis relative to its default rotation
                spineJoint.localRotation = defaultSpineRotation * Quaternion.Euler(0, spineOffset, 0);
            }
            if (neckJoint)
            {
                // Rotate the neck joint around its local Y axis relative to its default rotation
                neckJoint.localRotation = defaultNeckRotation * Quaternion.Euler(0, neckOffset, 0);
            }
        }
        else
        {
            // When CTRL is not held, smoothly return the arm and joints to their defaults

            // Smoothly reset the arm distance to default
            currentArmDistance = Mathf.Lerp(currentArmDistance, defaultArmDistance, Time.deltaTime * 5f);
            if (armIKTarget)
            {
                Vector3 defaultTargetPos = rightShoulder.position 
                    + (transform.forward * currentArmDistance) 
                    + (transform.right * armLateralOffset);
                armIKTarget.position = Vector3.Lerp(armIKTarget.position, defaultTargetPos, Time.deltaTime * 5f);
            }

            // Reset spine and neck rotations smoothly
            if (spineJoint)
            {
                spineJoint.localRotation = Quaternion.Slerp(spineJoint.localRotation, defaultSpineRotation, Time.deltaTime * 5f);
            }
            if (neckJoint)
            {
                neckJoint.localRotation = Quaternion.Slerp(neckJoint.localRotation, defaultNeckRotation, Time.deltaTime * 5f);
            }
        }
    }
}
