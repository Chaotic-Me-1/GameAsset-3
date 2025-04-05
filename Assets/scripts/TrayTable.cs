using System.Collections;
using UnityEngine;

public class TrayTable : MonoBehaviour
{
    [System.Serializable]
    public class ObjectRotation
    {
        public Transform targetObject;
        public Vector3 desiredRotation; // in Euler angles
    }

    public ObjectRotation[] targetsToRotate;

    private Rigidbody rb;

    void OnEnable()
    {
        // Apply rotations immediately
        foreach (var entry in targetsToRotate)
        {
            if (entry.targetObject != null)
            {
                entry.targetObject.localRotation = Quaternion.Euler(entry.desiredRotation);
            }
        }

        // Handle Rigidbody delay
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            StartCoroutine(EnablePhysicsAfterDelay(0.5f));
        }
    }

    private IEnumerator EnablePhysicsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;
    }
}