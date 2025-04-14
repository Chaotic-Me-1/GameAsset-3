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

    [Header("Memory Images")]
    public Image memoryImage;
    public Sprite memorySprite1;
    public Sprite memorySprite2;
    public float imageFadeDuration = 1f;

    [Header("Wake Up Timing")]
    public float wakeUpDelayAfterSecondMemory = 13f; // Default for this memory, set per memory object

    public KeyCode skipKey = KeyCode.Space;
    public KeyCode wakeUpKey = KeyCode.E;
    public int pressesToSkip = 10;
    private bool hintShouldFlash = false;
    [Range(0f, 10f)] public float hintFlashSpeed = 2f;
    [Range(0f, 1f)] public float hintMinAlpha = 0.2f;
    [Range(0f, 1f)] public float hintMaxAlpha = 1f;

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

        // Enable memory UI if disabled
        if (fadeImage != null)
        {
            var parentGO = fadeImage.transform.parent.gameObject;
            if (!parentGO.activeSelf)
                parentGO.SetActive(true);
            fadeImage.gameObject.SetActive(true);
        }

        if (hintText != null)
        {
            hintText.gameObject.SetActive(true);
            hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, hintMinAlpha);
            hintShouldFlash = true; // ✅ Start flashing right away
        }

        if (wakeUpText != null)
        {
            wakeUpText.gameObject.SetActive(false);
            wakeUpText.color = new Color(wakeUpText.color.r, wakeUpText.color.g, wakeUpText.color.b, 0f);
        }

        StartCoroutine(FadeVCAVolume(masterVCA, 1f, 0f, 2f));

        // 🎧 Memory start sound
        EventInstance memoryStart = default;
        if (!memoryStartSound.IsNull)
        {
            memoryStart = RuntimeManager.CreateInstance(memoryStartSound);
            RuntimeManager.AttachInstanceToGameObject(memoryStart, transform);
            memoryStart.start();

            if (memoryImage != null && memorySprite1 != null)
            {
                memoryImage.sprite = memorySprite1;
                memoryImage.color = new Color(1, 1, 1, 0);
                memoryImage.gameObject.SetActive(true);
                StartCoroutine(FadeImage(memoryImage, 0f, 1f, imageFadeDuration));
            }
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, alpha);
            if (hintText != null) hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (memoryStart.isValid())
        {
            memoryStart.getPlaybackState(out var state);
            while (state == PLAYBACK_STATE.PLAYING)
            {
                memoryStart.getPlaybackState(out state);
                yield return null;
            }
            memoryStart.release();
        }

        if (memoryImage != null)
            yield return StartCoroutine(FadeImage(memoryImage, 1f, 0f, imageFadeDuration));

        if (!skipTriggered && !memoryDeepSound.IsNull)
        {
            var deep = RuntimeManager.CreateInstance(memoryDeepSound);
            RuntimeManager.AttachInstanceToGameObject(deep, transform);
            deep.start();
            deep.release();

            if (memoryImage != null && memorySprite2 != null)
            {
                memoryImage.sprite = memorySprite2;
                memoryImage.color = new Color(1, 1, 1, 0);
                memoryImage.gameObject.SetActive(true);
                yield return StartCoroutine(FadeImage(memoryImage, 0f, 1f, imageFadeDuration));
            }
        }

        // After second image is faded in and sound played, wait before allowing wake-up
        if (!skipTriggered)
        {
            yield return new WaitForSeconds(wakeUpDelayAfterSecondMemory); // Delay based on this specific memory

            // Fade out the second image
            if (memoryImage != null)
                yield return StartCoroutine(FadeImage(memoryImage, 1f, 0f, imageFadeDuration));

            if (hintText != null)
            {
                hintText.gameObject.SetActive(false); // Hide skip text
                hintShouldFlash = false;
            }

            if (wakeUpText != null)
            {
                wakeUpText.gameObject.SetActive(true);
                yield return StartCoroutine(FadeText(wakeUpText, 0f, 1f, 1f)); // 1 second fade
            }

            canWakeUp = true;
        }
    }

    void Update()
    {
        if (!isMemoryActive) return;

        // 🔁 Flashing logic: Only active when skipping
        if (hintShouldFlash && hintText != null && hintText.gameObject.activeSelf)
        {
            float alpha = Mathf.Lerp(hintMinAlpha, hintMaxAlpha, Mathf.PingPong(Time.time * hintFlashSpeed, 1f));
            Color faceColor = hintText.color;
            hintText.color = new Color(faceColor.r, faceColor.g, faceColor.b, alpha);
        }

        if (!skipTriggered && Input.GetKeyDown(skipKey))
        {
            skipCounter++;

            if (skipCounter >= pressesToSkip)
            {
                skipTriggered = true;
                Debug.Log("Memory skipped early!");

                hintShouldFlash = false; // Stop flashing
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(false); // Hide completely now
                }

                if (wakeUpText != null)
                {
                    wakeUpText.gameObject.SetActive(true);
                    wakeUpText.color = new Color(wakeUpText.color.r, wakeUpText.color.g, wakeUpText.color.b, 1f);
                }

                canWakeUp = true;
            }
        }

        if (canWakeUp && Input.GetKeyDown(wakeUpKey))
        {
            ResetLoop();
            StartCoroutine(FadeVCAVolume(masterVCA, 0f, 1f, 2f, delay: 5f));
        }
    }

    private void ResetLoop()
    {
        if (memoryImage != null)
            StartCoroutine(FadeImage(memoryImage, 1f, 0f, imageFadeDuration));
        isMemoryActive = false;
        if (LoopCycleManager.instance != null)
            LoopCycleManager.instance.RestartScene();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    private IEnumerator FadeImage(Image image, float fromAlpha, float toAlpha, float duration)
    {
        float t = 0f;
        Color color = image.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
            image.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, toAlpha);
        if (toAlpha == 0f)
            image.gameObject.SetActive(false);
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float fromAlpha, float toAlpha, float duration)
    {
        float t = 0f;
        Color original = text.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
            text.color = new Color(original.r, original.g, original.b, a);
            yield return null;
        }

        text.color = new Color(original.r, original.g, original.b, toAlpha);
    }
}