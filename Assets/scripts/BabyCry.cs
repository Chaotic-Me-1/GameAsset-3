using UnityEngine;
using System.Collections;

public class BabyCry : MonoBehaviour
{
    public Animator animator;      // Assign the Animator in the Inspector
    public AudioSource crySound;   // Assign an AudioSource with the crying sound
    public float minCryInterval = 10f;  // Minimum time before crying again
    public float maxCryInterval = 30f;  // Maximum time before crying again
    public float cryDuration = 5f;      // How long the baby cries

    void Start()
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
        if (animator != null)
        {
            animator.SetBool("IsCrying", true);
        }
        if (crySound != null)
        {
            crySound.Play();
        }
    }

    private void StopCrying()
    {
        if (animator != null)
        {
            animator.SetBool("IsCrying", false);
        }
        if (crySound != null && crySound.isPlaying)
        {
            crySound.Stop();
        }
    }
}
