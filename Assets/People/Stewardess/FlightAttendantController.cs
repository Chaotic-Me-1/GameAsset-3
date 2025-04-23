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
            animator.SetBool("IsPushing", true);

            // keep going until we are very close to endPoint
            while (Vector3.Distance(transform.position, endPoint.position) > 0.05f)
            {
                // change direction every frame as to follow changes in pitch on the plane
                Vector3 dir = (endPoint.position - transform.position).normalized;

                // walk a small chunk then stop to serve
                float travelled = 0f;
                while (travelled < stopDistance &&
                    Vector3.Distance(transform.position, endPoint.position) > 0.05f)
                {
                    if (!waitingForPlayerChoice)
                    {
                        float step = moveSpeed * Time.deltaTime;
                        transform.position += dir * step;
                        travelled           += step;
                    }
                    yield return null;
                }

                // serve the row reached
                animator.SetBool("IsPushing", false);
                yield return ServePassengers();
                animator.SetBool("IsPushing", true);
            }

            /* reached end – pause, then teleport back to start */
            animator.SetBool("IsPushing", false);
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
