using UnityEngine;
using FMODUnity;
using TMPro;

// Script by OlafRT
// This script is the mysterious letter you get during the game.
// It acts similarly to the other interactable objects in the scene, like the leaflet,
// but also shows a dialogue when interacted with.

public class KarmaLetter : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [Tooltip("The dialogue data to open when the letter is picked up")]
    public DialogueData letterDialogue;

    [Header("HDRP glow")]
    public Color  emissionColor         = Color.white;
    public float  emissionNitsIntensity = 2000f;

    [Header("Audio")]
    public EventReference openLetterEvent;

    Renderer               rend;
    MaterialPropertyBlock  propBlock;

    void Start()
    {
        rend      = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        DisableGlow();
    }

    public void OnTouchStart() => EnableGlow();
    public void OnTouchEnd()   => DisableGlow();

    public void OnInteract()
    {
        if (letterDialogue == null)
        {
            Debug.LogWarning("LetterDialogueInteractable: No DialogueData");
            return;
        }

        DialogueData runtimeCopy = Instantiate(letterDialogue);
        InjectPlayerName(runtimeCopy);

        DialogueManager.instance.StartDialogue(runtimeCopy);

        if (!openLetterEvent.IsNull)
            RuntimeManager.PlayOneShot(openLetterEvent);

        // Disable interaction after opening, so we can't open it again.
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Disable the glow to show that its no longer active
        DisableGlow();
    }


    void InjectPlayerName(DialogueData data)
    {
        string playerName = PlayerNameManager.instance &&
                            !string.IsNullOrEmpty(PlayerNameManager.instance.playerName)
                            ? PlayerNameManager.instance.playerName
                            : "Passenger"; // passenger is the default name if no name was entered at hte main menu.

        // this gets replaced in the prompt text of the dialoguedata
        data.promptText = data.promptText.Replace("{NAME}", playerName);

        // this will replace inside each option / reaction if we want that too
        foreach (var opt in data.options)
        {
            if (opt == null) continue;
            opt.optionText   = opt.optionText.Replace  ("{NAME}", playerName);
            opt.reactionText = opt.reactionText.Replace("{NAME}", playerName);
        }
    }

    void EnableGlow()
    {
        if (rend == null) return;
        rend.GetPropertyBlock(propBlock);
        Color hdr = emissionColor.linear * emissionNitsIntensity;
        propBlock.SetColor("_EmissiveColor", hdr);
        propBlock.SetFloat("_EmissiveIntensity", emissionNitsIntensity);
        rend.SetPropertyBlock(propBlock);
    }

    void DisableGlow()
    {
        if (rend == null) return;
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissiveColor", Color.black);
        propBlock.SetFloat("_EmissiveIntensity", 0f);
        rend.SetPropertyBlock(propBlock);
    }
}