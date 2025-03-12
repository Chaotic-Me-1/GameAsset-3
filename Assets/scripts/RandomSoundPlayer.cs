using UnityEngine;
using System.Collections;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // Assign an AudioSource in the Inspector
    public float minInterval = 180f; // 3 minutes
    public float maxInterval = 600f; // 10 minutes

    void Start()
    {
        StartCoroutine(PlaySoundAtRandomIntervals());
    }

    private IEnumerator PlaySoundAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
