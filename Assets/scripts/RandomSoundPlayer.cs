using UnityEngine;
using System.Collections;
using FMODUnity; 
using FMOD.Studio;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private EventReference fmodEvent;

    public float minInterval = 180f; // 3 minutes
    public float maxInterval = 600f; // 10 minutes

    private void Start()
    {
        // Play sound when game starts
        if (!fmodEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(fmodEvent, transform.position);
        }

        // Continue with random-interval coroutine
        StartCoroutine(PlaySoundAtRandomIntervals());
    }

    private IEnumerator PlaySoundAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (!fmodEvent.IsNull)
            {
                RuntimeManager.PlayOneShot(fmodEvent, transform.position);
            }
        }
    }
}

