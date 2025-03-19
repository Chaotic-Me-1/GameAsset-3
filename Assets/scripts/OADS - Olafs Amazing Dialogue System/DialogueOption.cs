using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public int karmaImpact; // Positive, negative, or zero
    public string reactionText; // NPC reaction after choice
}

[System.Serializable]
public class DialogueData
{
    public string promptText; // Initial situation (e.g., "The baby is crying...")
    public DialogueOption[] options; // 3 options
}

