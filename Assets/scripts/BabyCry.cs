using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class BabyCry : MonoBehaviour
{
    [SerializeField]
    private Animator animator;   // Animator to make the baby cry

    [SerializeField]
    private EventReference cryEvent; // FMOD event

    public float minCryInterval = 10f;  // Minimum time before crying
    public float maxCryInterval = 30f;  // Maximum time before crying
    public float cryDuration = 5f;      // How long the baby cries

    private EventInstance cryInstance;

    // New logic variables
    private bool dialogueTriggered = false;
    private bool isCryingForever = false;
    private bool cryInterrupted = false;

    private void Start()
    {
        StartCoroutine(CryRoutine());
    }

    private IEnumerator CryRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minCryInterval, maxCryInterval);
            yield return new WaitForSeconds(waitTime);

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
            // If interrupted, StopCrying() was already called
        }
    }

    private void StartCrying()
    {
        if (animator != null)
        {
            animator.SetBool("IsCrying", true);
        }

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
        {
            animator.SetBool("IsCrying", false);
        }

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
        {
            animator.SetBool("IsCrying", true);
        }

        yield break; // Loop ends here; crying continues until manually stopped
    }

    private void TriggerBabyDialogue()
    {
        DialogueData babyDialogue = new DialogueData();
        babyDialogue.promptText = "A baby starts crying loudly nearby...";

        babyDialogue.options = new DialogueOption[3];

        babyDialogue.options[0] = new DialogueOption
        {
            optionText = "Gently reassure the baby.",
            karmaImpact = 10,
            reactionText = "The baby calms down, the parent gives you a thankful nod."
        };

        babyDialogue.options[1] = new DialogueOption
        {
            optionText = "Shout at the baby and the parents.",
            karmaImpact = -10,
            reactionText = "The crying intensifies. Other passengers glare at you."
        };

        babyDialogue.options[2] = new DialogueOption
        {
            optionText = "Ignore it and stay silent.",
            karmaImpact = 0,
            reactionText = "You do nothing. The crying continues unabated."
        };

        DialogueManager.instance.StartDialogue(babyDialogue, this); // Pass this BabyCry reference
    }

    // Called by DialogueManager after choice is made
    public void OnPlayerMadeChoice(int choiceIndex)
    {
        if (choiceIndex == 0) // Help baby
        {
            cryInterrupted = true;
            StopCrying();
        }
        else if (choiceIndex == 1) // Yell at baby
        {
            isCryingForever = true;
            Debug.Log("The baby is now crying forever...");
            // Current cry stops naturally, then CryForeverLoop begins
        }
        else
        {
            // Ignore: normal behavior
        }
    }
}

