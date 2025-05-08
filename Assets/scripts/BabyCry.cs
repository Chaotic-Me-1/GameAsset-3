using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

// Script by OlafRT
// This crying baby is a nuisance!
// After a delay that we set in the inspector, the baby starts crying, plays fMod audio, 
// and triggers dialogue. Depending on the players choice, crying may stop, 
// continue forever, or finish naturally. 

public class BabyCry : MonoBehaviour
{


    [SerializeField] private Animator animator;
    [SerializeField] private EventReference cryEvent;

    [Header("Timing")]
    public float delayBeforeCry = 5f; // Time after we enable the gameobject before the baby starts screeching horribly
    public float cryDuration = 5f;    // How long the baby cries for

    [Header("Dialogue")]
    public DialogueData cryingBabyDialogue;

    private EventInstance cryInstance;

    private bool dialogueTriggered = false;
    private bool isCryingForever = false;
    private bool cryInterrupted = false;

    void OnEnable()
    {
        StopAllCoroutines();
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
            Debug.LogWarning("You didn't put a DialogueData on the babycry you idiot!!", this);
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
            Debug.Log("The baby is now crying forever!! >:)");
            StopAllCoroutines();
            StopCrying();
            StartCoroutine(CryForeverLoop());
        }
        else if (choiceIndex == 2)
        {
            // Just let the baby finish crying naturally
        }
    }
}
