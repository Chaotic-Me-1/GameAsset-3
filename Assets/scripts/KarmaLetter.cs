using UnityEngine;
using FMODUnity;
using TMPro;

public class KarmaLetter : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [Tooltip("The dialogue to open when the letter is picked up")]
    public DialogueData letterDialogue;

    [Header("Glow (HDRP)")]
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
            Debug.LogWarning("LetterDialogueInteractable: No DialogueData assigned!");
            return;
        }

        // optional: personalise with the player’s name
        DialogueData runtimeCopy = Instantiate(letterDialogue);
        InjectPlayerName(runtimeCopy);

        DialogueManager.instance.StartDialogue(runtimeCopy);

        if (!openLetterEvent.IsNull)
            RuntimeManager.PlayOneShot(openLetterEvent);
    }


    void InjectPlayerName(DialogueData data)
    {
        string playerName = PlayerNameManager.instance &&
                            !string.IsNullOrEmpty(PlayerNameManager.instance.playerName)
                            ? PlayerNameManager.instance.playerName
                            : "Passenger";

        // replace in prompt
        data.promptText = data.promptText.Replace("{NAME}", playerName);

        // replace inside each option / reaction
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