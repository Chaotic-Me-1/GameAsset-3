using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;

    void OnMouseDown() // Or trigger interaction however you like
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
}
