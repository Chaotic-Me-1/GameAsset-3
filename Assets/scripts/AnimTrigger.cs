using System.Collections;
using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    [Header("Target Animator")]
    [Tooltip("GameObject that has (or is) the Animator")]
    public GameObject targetObject;

    [Tooltip("Trigger name in the Animator.")]
    public string triggerName = "Play";

    [Header("Delay")]
    [Min(0f)]
    public float delayBeforePlay = 0f;

    Animator anim;

    void Awake()
    {
        if (targetObject != null)
            anim = targetObject.GetComponent<Animator>();

        if (anim == null)
            Debug.LogWarning($"{name}: No Animator!!!!.");
    }

    void OnEnable()
    {
        if (anim == null) return;

        if (delayBeforePlay <= 0f)
            TriggerAnimation();
        else
            StartCoroutine(DelayedPlay());
    }

    IEnumerator DelayedPlay()
    {
        yield return new WaitForSeconds(delayBeforePlay);
        TriggerAnimation();
    }

    void TriggerAnimation()
    {
        if (string.IsNullOrEmpty(triggerName))
            anim.Play(0);
        else
            anim.SetTrigger(triggerName);
    }
}