using UnityEngine;
using System.Collections;
using FMODUnity;

public class FlightAttendantController : MonoBehaviour
{
    public Animator animator;
    public Transform startPoint;
    public Transform endPoint;

    public float moveSpeed = 1f;
    public float stopDistance = 2f;
    public float stopDuration = 2f;

    private bool movingToEnd = true;
    private bool dialogueTriggered = false;
    private bool waitingForPlayerChoice = false;
    public DialogueData[] possibleDialogues;

    void Start()
    {
        transform.position = startPoint.position;
        StartCoroutine(ServiceRoutine());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!dialogueTriggered && other.CompareTag("PlayerSeat"))
        {
            waitingForPlayerChoice = true;
            dialogueTriggered = true;
            animator.SetBool("IsPushing", false);
            animator.SetBool("IsTalking", true);

            TriggerFlightAttendantDialogue();
        }
    }

    private void TriggerFlightAttendantDialogue()
    {
        int currentLoop = LoopCycleManager.instance != null ? LoopCycleManager.instance.loopCount : 0;

        DialogueData selectedDialogue = null;

        foreach (var d in possibleDialogues)
        {
            if (d != null && (d.requiredLoop == -1 || d.requiredLoop == currentLoop))
            {
                selectedDialogue = d;
                break;
            }
        }

        if (selectedDialogue != null)
        {
            DialogueManager.instance.StartDialogue(selectedDialogue, null);
            StartCoroutine(WaitForDialogueToFinish());
        }
        else
        {
            Debug.LogWarning($"No flight attendant dialogue found for loop {currentLoop}!");
        }
    }

    private IEnumerator WaitForDialogueToFinish()
    {
        // Wait until the dialogue UI is hidden
        while (DialogueManager.instance.dialoguePanel.activeInHierarchy)
        {
            yield return null;
        }

        animator.SetBool("IsTalking", false);
        animator.SetBool("IsPushing", true);
        waitingForPlayerChoice = false;
    }

    private IEnumerator ServiceRoutine()
    {
        while (true)
        {
            // Calculate direction
            Vector3 direction = (endPoint.position - startPoint.position).normalized;

            // Distance between start and end
            float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);

            // Track how far we've moved along the path
            float distanceMoved = 0f;

            // Set animation to pushing
            animator.SetBool("IsPushing", true);

            while (distanceMoved < totalDistance)
            {
                // Move by small increments
                Vector3 nextStop = transform.position + direction * stopDistance;

                // Clamp so we don’t overshoot endPoint
                if (Vector3.Distance(nextStop, startPoint.position) > totalDistance)
                {
                    nextStop = endPoint.position;
                }

                while (Vector3.Distance(transform.position, nextStop) > 0.05f)
                {
                    if (!waitingForPlayerChoice)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, nextStop, moveSpeed * Time.deltaTime);
                    }
                    yield return null;
                }

                distanceMoved += stopDistance;

                // Stop and serve passengers
                animator.SetBool("IsPushing", false);
                yield return ServePassengers();
                animator.SetBool("IsPushing", true);
            }

            // Reached end, reset to start after short pause
            animator.SetBool("IsPushing", false);
            Debug.Log("Reached end, resetting...");
            yield return new WaitForSeconds(1f);

            transform.position = startPoint.position;
        }
    }

    private IEnumerator ServePassengers()
    {
        animator.SetBool("IsTalking", true);
        yield return new WaitForSeconds(GetAnimationLength("Talk"));
        animator.SetBool("IsTalking", false);

        if (Random.value > 0.5f)
        {
            animator.SetBool("IsServing", true);
            yield return new WaitForSeconds(GetAnimationLength("Serve"));
            animator.SetBool("IsServing", false);
        }

        yield return new WaitForSeconds(stopDuration);
    }

    private float GetAnimationLength(string animationName)
    {
        AnimationClip clip = GetAnimationClip(animationName);
        return clip != null ? clip.length : 2f;
    }

    private AnimationClip GetAnimationClip(string name)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name) return clip;
        }
        return null;
    }
}
