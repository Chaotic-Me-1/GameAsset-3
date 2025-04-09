using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    public MovementTutorial tutorial;
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
                interact.OnTouchStart();
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
                interact.OnTouchEnd(); 
                Debug.Log("Stopped touching: " + other.name);
            }

            currentInteractable = null;
        }
    }

    void Update()
    {
        if (currentInteractable != null && Input.GetMouseButtonDown(0))
        {
            IInteractable interact = currentInteractable.GetComponent<IInteractable>();
            if (interact != null)
            {
                //  Notify tutorial manually
                if (tutorial != null)
                {
                    tutorial.MarkAsInteracted();
                }

                //  Inject hand target if drink
                if (interact is DrinkInteractable drink)
                {
                    drink.followTarget = GameObject.FindWithTag("PlayerHand")?.transform;
                }

                interact.OnInteract();
            }
        }
    }
}
