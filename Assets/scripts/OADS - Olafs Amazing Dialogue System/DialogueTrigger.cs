using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData[] possibleDialogues;
    public float triggerDelay = 1f;

    void OnEnable()
    {
        StartCoroutine(TriggerDialogueAfterDelay());
    }

    private System.Collections.IEnumerator TriggerDialogueAfterDelay()
    {
        yield return new WaitForSeconds(triggerDelay);

        int currentLoop = LoopCycleManager.instance != null ? LoopCycleManager.instance.loopCount : 0;

        foreach (var d in possibleDialogues)
        {
            if (d != null && (d.requiredLoop == -1 || d.requiredLoop == currentLoop))
            {
                DialogueManager.instance.StartDialogue(d);
                yield break;
            }
        }

        Debug.LogWarning("No valid dialogue found for loop: " + currentLoop);
    }
}