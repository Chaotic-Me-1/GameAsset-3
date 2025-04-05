using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData[] possibleDialogues;

    void OnMouseDown()
    {
        int currentLoop = LoopCycleManager.instance != null ? LoopCycleManager.instance.loopCount : 0;

        // Find a dialogue that matches this loop (or is -1 for "any")
        foreach (var d in possibleDialogues)
        {
            if (d != null && (d.requiredLoop == -1 || d.requiredLoop == currentLoop))
            {
                DialogueManager.instance.StartDialogue(d);
                return;
            }
        }

        Debug.LogWarning("No valid dialogue found for loop: " + currentLoop);
    }
}
