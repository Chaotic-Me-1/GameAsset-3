using UnityEngine;

public class HandGrab : MonoBehaviour
{
    [Header("Joints")]
    public Transform thumbJoint;
    public Transform[] fingerJoints;

    [Header("Grab Rotations")]
    public Quaternion thumbOpenRotation = Quaternion.Euler(27.7707577f, 23.6091518f, 53.514492f);
    public Quaternion thumbClosedRotation = Quaternion.Euler(27.7707729f, 23.6091499f, 14.9749012f);

    public Quaternion fingerOpenRotation = Quaternion.Euler(35.0690994f, 357.453796f, 351.542053f);
    public Quaternion fingerClosedRotation = Quaternion.Euler(74.3124695f, 336.757294f, 333.563019f);

    [Header("Smoothing")]
    public float grabSmoothness = 10f;

    private float grabBlend = 0f; // 0 = open, 1 = closed

    void LateUpdate()
    {
        // Automatically update grab state based on left mouse
        bool isGrabbing = Input.GetMouseButton(0);

        float target = isGrabbing ? 1f : 0f;
        grabBlend = Mathf.Lerp(grabBlend, target, Time.deltaTime * grabSmoothness);

        // Thumb
        if (thumbJoint)
            thumbJoint.localRotation = Quaternion.Lerp(thumbOpenRotation, thumbClosedRotation, grabBlend);

        // Fingers
        foreach (Transform finger in fingerJoints)
        {
            if (finger)
                finger.localRotation = Quaternion.Lerp(fingerOpenRotation, fingerClosedRotation, grabBlend);
        }
    }
}

