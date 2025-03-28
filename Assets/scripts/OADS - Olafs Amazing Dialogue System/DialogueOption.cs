using UnityEngine;
using FMODUnity;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public int karmaImpact;
    public string reactionText;

    public EventReference playerVoiceLine;
    public EventReference npcReactionVoiceLine;
}
