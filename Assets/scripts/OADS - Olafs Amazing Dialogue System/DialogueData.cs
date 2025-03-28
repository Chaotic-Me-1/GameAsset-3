using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string promptText;
    public EventReference promptVoiceLine;

    public DialogueOption[] options;
}

