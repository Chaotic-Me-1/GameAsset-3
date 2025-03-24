using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    private Collider currentInteractable;
    public bool IsTouchingInteractable()
    {
        return currentInteractable != null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            currentInteractable = other;

            IInteractable interact = currentInteractable.GetComponent<IInteractable>();
            if (interact != null)
            {
                interact.OnTouchStart();  // 🔥 THIS fires glow
                Debug.Log("Touching: " + other.name);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == currentInteractable)
        {
            IInteractable interact = currentInteractable.GetComponent<IInteractable>();
            if (interact != null)
            {
                interact.OnTouchEnd();  // 🔥 THIS disables glow
                Debug.Log("Stopped touching: " + other.name);
            }

            currentInteractable = null;
        }
    }

    void Update()
    {
        if (currentInteractable != null && Input.GetMouseButtonDown(0)) // Left click
        {
            Debug.Log("Interacting with: " + currentInteractable.name);
            // Optional: Call interaction method on object
            IInteractable interact = currentInteractable.GetComponent<IInteractable>();
            if (interact != null)
            {
                interact.OnInteract();
            }
        }
    }
}
