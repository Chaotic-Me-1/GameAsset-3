using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MysteriousCompartment : MonoBehaviour
{
    [Header("FMOD Knock Sounds")]
    public EventReference knock1;
    public EventReference knock2;
    public EventReference knock3;
    public EventReference finalKnock;

    [Header("Animation")]
    public Animator compartmentAnimator; // The main Animator that plays the opening animation
    public string triggerName = "OpenCompartment";

    [Header("Flapping Target")]
    public GameObject animatedChildWithAnimator; // This child has the wings + animator
    public GameObject postAnimationObject; // This becomes active after 10s
    public float postAnimDelay = 10f;

    [Header("Loop Control")]
    public int triggerOnLoop = 4;

    private bool hasTriggered = false;

    void Start()
    {
        if (LoopCycleManager.instance != null && LoopCycleManager.instance.loopCount == triggerOnLoop)
        {
            StartCoroutine(KnockingSequence());
        }
    }

    private IEnumerator KnockingSequence()
    {
        if (hasTriggered) yield break;
        hasTriggered = true;

        yield return new WaitForSeconds(2f);
        PlayOneShot(knock1);

        yield return new WaitForSeconds(3f);
        PlayOneShot(knock2);

        yield return new WaitForSeconds(2f);
        PlayOneShot(knock3);

        yield return new WaitForSeconds(1.5f);
        PlayOneShot(finalKnock);

        if (compartmentAnimator != null)
            compartmentAnimator.SetTrigger(triggerName);

        yield return new WaitForSeconds(postAnimDelay);

        if (animatedChildWithAnimator != null)
        {
            Animator anim = animatedChildWithAnimator.GetComponent<Animator>();
            if (anim != null)
                anim.enabled = false;
        }

        if (postAnimationObject != null)
            postAnimationObject.SetActive(true);
    }

    private void PlayOneShot(EventReference sound)
    {
        if (!sound.IsNull)
        {
            EventInstance instance = RuntimeManager.CreateInstance(sound);
            RuntimeManager.AttachInstanceToGameObject(instance, transform);
            instance.start();
            instance.release();
        }
    }
}