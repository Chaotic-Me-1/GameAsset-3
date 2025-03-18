using UnityEngine;
using System.Collections;

public class FlightAttendantController : MonoBehaviour
{
    public Animator animator;
    public Transform startPoint;
    public Transform endPoint;

    public float moveSpeed = 1f;
    public float stopDistance = 2f;
    public float stopDuration = 2f;

    private Vector3 direction;
    private Vector3 nextStop;
    private bool isMoving = true;

    void Start()
    {
        transform.position = startPoint.position;
        direction = (endPoint.position - startPoint.position).normalized;
        StartCoroutine(ServiceRoutine());
    }

    private IEnumerator ServiceRoutine()
    {
        while (true) // Loop forever
        {
            nextStop = startPoint.position;

            while (Vector3.Distance(transform.position, endPoint.position) > 0.1f)
            {
                yield return MoveToNextRow();
                yield return ServePassengers();
            }

            // ✅ Fix: Stop movement and reset position
            Debug.Log("Reached end! Resetting to start...");
            transform.position = startPoint.position; 
        }
    }

    private IEnumerator MoveToNextRow()
    {
        isMoving = true;
        animator.SetBool("IsPushing", true);

        nextStop += direction * stopDistance;

        while (Vector3.Distance(transform.position, nextStop) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextStop, moveSpeed * Time.deltaTime);
            yield return null;
        }

        animator.SetBool("IsPushing", false);
        isMoving = false;
    }

    private IEnumerator ServePassengers()
    {
        animator.SetBool("IsPushing", false);
        animator.SetBool("IsTalking", true);

        yield return new WaitForSeconds(GetAnimationLength("Talk"));

        animator.SetBool("IsTalking", false);

        if (Random.value > 0.5f) 
        {
            animator.SetBool("IsServing", true);
            yield return new WaitForSeconds(GetAnimationLength("Serve"));
            animator.SetBool("IsServing", false);
        }

        animator.SetBool("IsPushing", true);
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
