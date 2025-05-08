using UnityEngine;
using System.Collections;
using FMODUnity; 
using FMOD.Studio;

// Script by OlafRT
// Plays those cabin chime sounds everyone once in a while, depending on how much we set the intervals to. Also plays it at the start of the game.

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private EventReference fmodEvent;

    public float minInterval = 180f;
    public float maxInterval = 600f;

    private void Start()
    {
        // Play the chime sound when the game starts
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

