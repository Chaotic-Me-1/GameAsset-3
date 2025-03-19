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

    private bool movingToEnd = true;

    void Start()
    {
        transform.position = startPoint.position;
        StartCoroutine(ServiceRoutine());
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

                // Move to nextStop
                while (Vector3.Distance(transform.position, nextStop) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextStop, moveSpeed * Time.deltaTime);
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
