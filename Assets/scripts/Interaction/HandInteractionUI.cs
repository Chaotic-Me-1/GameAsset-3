using UnityEngine;
using UnityEngine.UI;

public class HandInteractionUI : MonoBehaviour
{
    public Image handIcon;
    private HandInteraction handInteraction;

    void Start()
    {
        handInteraction = GetComponent<HandInteraction>();
        handIcon.enabled = false;
    }

    void Update()
    {
        if (handInteraction.IsTouchingInteractable())
        {
            handIcon.enabled = true;
        }
        else
        {
            handIcon.enabled = false;
        }
    }
}
