using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class BabyCry : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EventReference cryEvent;

    [Header("Timing")]
    public float delayBeforeCry = 5f; // ⏱ Time after enable before crying starts
    public float cryDuration = 5f;    // How long the baby cries

    [Header("Dialogue")]
    public DialogueData cryingBabyDialogue;

    private EventInstance cryInstance;

    private bool dialogueTriggered = false;
    private bool isCryingForever = false;
    private bool cryInterrupted = false;

    void OnEnable()
    {
        StopAllCoroutines(); // In case it's already running
        StartCoroutine(DelayedCryStart());
    }

    private IEnumerator DelayedCryStart()
    {
        yield return new WaitForSeconds(delayBeforeCry);
        StartCrying();

        float timer = 0f;
        while (timer < cryDuration && !cryInterrupted)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!cryInterrupted)
        {
            StopCrying();
        }
    }

    private void StartCrying()
    {
        if (animator != null)
            animator.SetBool("IsCrying", true);

        if (!cryEvent.IsNull)
        {
            cryInstance = RuntimeManager.CreateInstance(cryEvent);
            RuntimeManager.AttachInstanceToGameObject(cryInstance, transform, GetComponent<Rigidbody>());
            cryInstance.start();
        }

        if (!dialogueTriggered)
        {
            TriggerBabyDialogue();
            dialogueTriggered = true;
        }
    }

    private void StopCrying()
    {
        if (animator != null)
            animator.SetBool("IsCrying", false);

        if (cryInstance.isValid())
        {
            cryInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            cryInstance.release();
            cryInstance.clearHandle();
        }

        dialogueTriggered = false;
        cryInterrupted = false;

        if (isCryingForever)
        {
            StartCoroutine(CryForeverLoop());
        }
    }

    private IEnumerator CryForeverLoop()
    {
        if (!cryEvent.IsNull)
        {
            cryInstance = RuntimeManager.CreateInstance(cryEvent);
            RuntimeManager.AttachInstanceToGameObject(cryInstance, transform, GetComponent<Rigidbody>());
            cryInstance.start();
        }

        if (animator != null)
            animator.SetBool("IsCrying", true);

        yield break;
    }

    private void TriggerBabyDialogue()
    {
        if (cryingBabyDialogue != null)
        {
            DialogueManager.instance.StartDialogue(cryingBabyDialogue, this);
        }
        else
        {
            Debug.LogWarning("No DialogueData assigned to BabyCry!", this);
        }
    }

    public void OnPlayerMadeChoice(int choiceIndex)
    {
        if (choiceIndex == 0)
        {
            cryInterrupted = true;
            StopCrying();
        }
        else if (choiceIndex == 1)
        {
            isCryingForever = true;
            Debug.Log("The baby is now crying forever! >:)");
            StopAllCoroutines();
            StopCrying();
            StartCoroutine(CryForeverLoop());
        }
        else if (choiceIndex == 2)
        {
            // Let it finish naturally
        }
    }
}
