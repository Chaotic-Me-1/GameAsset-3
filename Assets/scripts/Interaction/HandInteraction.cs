using UnityEngine;
using System.Collections.Generic;

// Script by OlafRT
// Let's us interact with things using our hand.
// If you are touching something and click it, it interacts.

public class HandInteraction : MonoBehaviour
{
    public MovementTutorial tutorial;
    public float detectionRadius = 0.05f;
    public LayerMask interactableLayer;

    private IInteractable currentScript = null;
    private Collider currentCollider = null;

    private readonly Collider[] results = new Collider[5];

    void Update()
    {
        // During dialogue or magazine interaction, we don't want to interact
        if (GameState.IsMagazineOpen || DialogueManager.IsDialogueActive)
            return;

        int hits = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, results, interactableLayer);

        Collider newCollider = null;
        IInteractable newScript = null;

        for (int i = 0; i < hits; i++)
        {
            if (results[i].CompareTag("Interactable"))
            {
                newCollider = results[i];
                newScript = newCollider.GetComponent<IInteractable>();
                break;
            }
        }

        // If we've touched something new
        if (newScript != currentScript)
        {
            if (currentScript != null)
            {
                currentScript.OnTouchEnd();
                Debug.Log("Touch ended: " + currentCollider?.name);
            }

            if (newScript != null)
            {
                newScript.OnTouchStart();
                Debug.Log("Touch started: " + newCollider.name);
            }

            currentScript = newScript;
            currentCollider = newCollider;
        }

        if (currentScript != null && Input.GetMouseButtonDown(0))
        {
            tutorial?.MarkAsInteracted();

            if (currentScript is DrinkInteractable drink)
                drink.followTarget = GameObject.FindWithTag("PlayerHand")?.transform;

            currentScript.OnInteract();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
    public bool IsTouchingInteractable()
    {
        return currentScript != null;
    }
}