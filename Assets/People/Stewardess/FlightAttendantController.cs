using UnityEngine;
using System.Collections;

public class FlightAttendantController : MonoBehaviour
{
    public Animator animator;
    public Transform startPoint;
    public Transform endPoint;

    public float moveSpeed = 1f;       // Speed when walking
    public float stopDistance = 2f;    // Distance between rows
    public float stopDuration = 2f;    // Wait time for talking/serving

    private Vector3 direction; // Ensures movement in a straight line
    private Vector3 nextStop;
    private bool isMoving = true;

    void Start()
    {
        transform.position = startPoint.position; // Start at the first position
        direction = (endPoint.position - startPoint.position).normalized; // Ensure straight movement
        StartCoroutine(ServiceRoutine());
    }

    private IEnumerator ServiceRoutine()
    {
        while (true) // Infinite loop for continuous service
        {
            nextStop = startPoint.position;

            while (Vector3.Distance(transform.position, endPoint.position) > 0.1f)
            {
                yield return MoveToNextRow();
                yield return ServePassengers();
            }

            // ✅ Reset to start correctly
            ResetToStart();
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
        // Stop walking
        animator.SetBool("IsPushing", false);
        animator.SetBool("IsTalking", true);

        yield return new WaitForSeconds(GetAnimationLength("Talk"));
        
        animator.SetBool("IsTalking", false);

        if (Random.value > 0.5f) // 50% chance to serve food
        {
            animator.SetBool("IsServing", true);
            yield return new WaitForSeconds(GetAnimationLength("Serve"));

            animator.SetBool("IsServing", false);
        }

        // Always go back to pushing after talking/serving
        animator.SetBool("IsPushing", true);
    }

    private void ResetToStart()
    {
        Debug.Log("Resetting to start position!"); // Debugging
        transform.position = startPoint.position;
        nextStop = startPoint.position;
    }

    private float GetAnimationLength(string animationName)
    {
        AnimationClip clip = GetAnimationClip(animationName);
        return clip != null ? clip.length : 2f; // Default to 2 seconds if not found
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
