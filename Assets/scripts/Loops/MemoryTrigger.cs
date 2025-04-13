using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class MemoryTrigger : MonoBehaviour
{
    public float triggerDelay = 10f;
    public Image fadeImage;
    public TextMeshProUGUI hintText;
    public TextMeshProUGUI wakeUpText;

    [Header("FMOD Events")]
    public EventReference memoryStartSound;
    public EventReference memoryDeepSound;

    [Header("Wakeup")]
    public EventReference wakeUpSound;

    public KeyCode skipKey = KeyCode.Space;
    public KeyCode wakeUpKey = KeyCode.E;
    public int pressesToSkip = 10;

    private bool isMemoryActive = false;
    private bool skipTriggered = false;
    private bool canWakeUp = false;
    private float skipCounter = 0f;

    public VCA masterVCA;
    public VCA memoryVCA;

    void Awake()
    {
        masterVCA = RuntimeManager.GetVCA("vca:/Master");
        memoryVCA = RuntimeManager.GetVCA("vca:/Memory");
    }

    void OnEnable()
    {
        StartCoroutine(BeginMemoryAfterDelay());
    }

    private IEnumerator BeginMemoryAfterDelay()
    {
        yield return new WaitForSeconds(triggerDelay);
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        isMemoryActive = true;
        skipTriggered = false;
        canWakeUp = false;
        skipCounter = 0f;

        float fadeDuration = 3f;
        float t = 0f;

        StartCoroutine(FadeVCAVolume(masterVCA, 1f, 0f, 2f)); // Fade down rest of game audio

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(true);

        if (hintText != null)
            hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, 0f);

        if (wakeUpText != null)
        {
            wakeUpText.gameObject.SetActive(false); // hide initially
            wakeUpText.color = new Color(wakeUpText.color.r, wakeUpText.color.g, wakeUpText.color.b, 0f);
        }

        // 🎧 Memory start sound
        if (!memoryStartSound.IsNull)
        {
            var sfx = RuntimeManager.CreateInstance(memoryStartSound);
            RuntimeManager.AttachInstanceToGameObject(sfx, transform);
            sfx.start();
            sfx.release();
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);

            if (fadeImage != null)
                fadeImage.color = new Color(0, 0, 0, alpha);

            if (hintText != null)
                hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, alpha);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (!skipTriggered && !memoryDeepSound.IsNull)
        {
            var deep = RuntimeManager.CreateInstance(memoryDeepSound);
            RuntimeManager.AttachInstanceToGameObject(deep, transform);
            deep.start();
            deep.release();
        }

        if (wakeUpText != null)
        {
            wakeUpText.gameObject.SetActive(true);
            wakeUpText.color = new Color(wakeUpText.color.r, wakeUpText.color.g, wakeUpText.color.b, 1f);
        }

        canWakeUp = true;
    }

    void Update()
    {
        if (!isMemoryActive) return;

        if (!skipTriggered && Input.GetKeyDown(skipKey))
        {
            skipCounter++;

            if (hintText != null)
                hintText.gameObject.SetActive(true);

            if (skipCounter >= pressesToSkip)
            {
                skipTriggered = true;
                Debug.Log("Memory skipped early!");
            }
        }

        if (canWakeUp && Input.GetKeyDown(wakeUpKey))
        {
            PlayWakeUpSound();
            ResetLoop();
            StartCoroutine(FadeVCAVolume(masterVCA, 0f, 1f, 2f, delay: 5f)); // Restore after memory
        }
    }

    private void ResetLoop()
    {
        isMemoryActive = false;

        if (LoopCycleManager.instance != null)
            LoopCycleManager.instance.RestartScene();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // fallback
    }
    
    private IEnumerator FadeVCAVolume(VCA vca, float from, float to, float duration, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float volume = Mathf.Lerp(from, to, t / duration);
            vca.setVolume(volume);
            yield return null;
        }

        vca.setVolume(to);
    }

    private void PlayWakeUpSound()
    {
        if (!wakeUpSound.IsNull)
        {
            var sfx = RuntimeManager.CreateInstance(wakeUpSound);
            RuntimeManager.AttachInstanceToGameObject(sfx, transform);
            sfx.start();
            sfx.release();
        }
    }
}