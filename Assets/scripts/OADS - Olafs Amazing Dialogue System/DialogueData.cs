using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string promptText;
    public EventReference promptVoiceLine;

    public DialogueOption[] options;

    [Header("Loop Requirements")]
    public int requiredLoop = -1; // -1 means available on all loops
}

