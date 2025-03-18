using UnityEngine;
using System.Collections;
using FMODUnity; 
using FMOD.Studio;

public class BabyCry: MonoBehaviour
{
    [SerializeField]
    private Animator animator;   // Animator to make the baby cry
    
    [SerializeField]
    private EventReference cryEvent; // FMOD event

    public float minCryInterval = 10f;  // Minimum time before crying
    public float maxCryInterval = 30f;  // Maximum time before crying
    public float cryDuration = 5f;      // How long the baby cries

    private EventInstance cryInstance;

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
            yield return new WaitForSeconds(cryDuration);
            StopCrying();
        }
    }

    private void StartCrying()
    {
        // Trigger animator
        if (animator != null)
        {
            animator.SetBool("IsCrying", true);
        }

        // Create and start FMOD event
        if (!cryEvent.IsNull)
        {
            cryInstance = RuntimeManager.CreateInstance(cryEvent);

            RuntimeManager.AttachInstanceToGameObject(
                cryInstance, 
                transform, 
                GetComponent<Rigidbody>() // or null if no Rigidbody
            );

            cryInstance.start();
        }
    }

    private void StopCrying()
    {
        // Stop the animators crying animation
        if (animator != null)
        {
            animator.SetBool("IsCrying", false);
        }

        // Stop, release FMOD event
        if (cryInstance.isValid())
        {
            cryInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            cryInstance.release();
            cryInstance.clearHandle();
        }
    }
}

