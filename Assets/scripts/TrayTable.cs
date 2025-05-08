using System.Collections;
using UnityEngine;

// Script by OlafRT
// Rotates the tray table stuff when the drink is served so the drink has somewhere to stay!
// otherwise it would just fall right to the floor, you know...

public class TrayTable : MonoBehaviour
{
    [System.Serializable]
    public class ObjectRotation
    {
        public Transform targetObject;
        public Vector3 desiredRotation;
    }

    public ObjectRotation[] targetsToRotate;

    private Rigidbody rb;

    void OnEnable()
    {
        // Rotate immediately
        foreach (var entry in targetsToRotate)
        {
            if (entry.targetObject != null)
            {
                entry.targetObject.localRotation = Quaternion.Euler(entry.desiredRotation);
            }
        }

        // Rigidbody delay so that the drink cant clip through the table.
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