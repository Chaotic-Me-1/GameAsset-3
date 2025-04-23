using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class MemoryToCredits : MonoBehaviour
{
    [Header("Timings")]
    public float triggerDelay = 10f;
    public float wakeUpDelayAfterSecond = 13f;

    [Header("Images / Text")]
    public Image fadeImage;
    public Image memoryImage;
    public Sprite memorySprite1;
    public Sprite memorySprite2;
    public float imageFadeDuration = 1f;

    public TextMeshProUGUI hintText;
    public TextMeshProUGUI wakeUpText;

    [Header("FMOD Events")]
    public EventReference memoryStartSound;
    public EventReference memoryDeepSound;

    [Header("Input")]
    public KeyCode skipKey  = KeyCode.Space;
    public KeyCode wakeKey  = KeyCode.E;
    public int     pressesToSkip = 10;

    [Header("Fade‑to‑credits")]
    public float screenFadeDuration = 3f;
    public string creditsSceneName  = "Credits";

    bool memoryActive, skipTriggered, canWakeUp;
    int  skipCount;

    VCA masterVCA, memoryVCA;

    void Awake()
    {
        masterVCA = RuntimeManager.GetVCA("vca:/Master");
        memoryVCA = RuntimeManager.GetVCA("vca:/Memory");
    }

    void OnEnable() => StartCoroutine(BeginMemoryAfterDelay());

    IEnumerator BeginMemoryAfterDelay()
    {
        yield return new WaitForSeconds(triggerDelay);
        StartCoroutine(MemoryRoutine());
    }

    IEnumerator MemoryRoutine()
    {
        memoryActive = true;
        skipTriggered = canWakeUp = false;
        skipCount = 0;

        if (fadeImage) { fadeImage.gameObject.SetActive(true); fadeImage.color = Color.clear; }
        if (hintText)  { hintText.gameObject.SetActive(true);  hintText.alpha = 0; }
        if (wakeUpText){ wakeUpText.gameObject.SetActive(false); }

        StartCoroutine(FadeVCA(masterVCA, 1, 0, 2));

        EventInstance startEvt = default;
        if (!memoryStartSound.IsNull)
        {
            startEvt = RuntimeManager.CreateInstance(memoryStartSound);
            RuntimeManager.AttachInstanceToGameObject(startEvt, transform);
            startEvt.start();
        }
        if (memoryImage && memorySprite1)
        {
            memoryImage.sprite = memorySprite1;
            memoryImage.color  = new Color(1,1,1,0);
            memoryImage.gameObject.SetActive(true);
            yield return FadeImage(memoryImage, 0, .6f, imageFadeDuration);
        }

        yield return FadeScreen(new Color(.5f,.5f,.5f,1), 3);

        if (startEvt.isValid())
        {
            PLAYBACK_STATE st;
            startEvt.getPlaybackState(out st);
            while (st == PLAYBACK_STATE.PLAYING)
            {
                startEvt.getPlaybackState(out st);
                yield return null;
            }
            startEvt.release();
        }

        if (memoryImage) yield return FadeImage(memoryImage, .6f, 0, imageFadeDuration);

        if (!skipTriggered && !memoryDeepSound.IsNull)
        {
            var deep = RuntimeManager.CreateInstance(memoryDeepSound);
            RuntimeManager.AttachInstanceToGameObject(deep, transform);
            deep.start(); deep.release();

            if (memoryImage && memorySprite2)
            {
                memoryImage.sprite = memorySprite2;
                memoryImage.color  = new Color(1,1,1,0);
                memoryImage.gameObject.SetActive(true);
                yield return FadeImage(memoryImage, 0, .6f, imageFadeDuration);
            }
        }

        if (!skipTriggered)
        {
            yield return new WaitForSeconds(wakeUpDelayAfterSecond);

            if (memoryImage) yield return FadeImage(memoryImage, .6f, 0, imageFadeDuration);

            if (hintText)  hintText.gameObject.SetActive(false);
            if (wakeUpText)
            {
                wakeUpText.gameObject.SetActive(true);
                yield return FadeText(wakeUpText, 0, 1, 1);
            }
            canWakeUp = true;
        }
    }

    void Update()
    {
        if (!memoryActive) return;

        if (hintText && hintText.gameObject.activeSelf)
        {
            float a = Mathf.Lerp(.2f, 1f, Mathf.PingPong(Time.time*2,1));
            hintText.alpha = a;
        }

        if (!skipTriggered && Input.GetKeyDown(skipKey))
        {
            skipCount++;
            if (skipCount >= pressesToSkip) { skipTriggered = true; FinishMemory(); }
        }

        if (canWakeUp && Input.GetKeyDown(wakeKey))
            FinishMemory();
    }

    void FinishMemory()
    {
        memoryActive = false;
        RuntimeManager.StudioSystem.release();
        StartCoroutine(FadeToCredits());
    }

    IEnumerator FadeToCredits()
    {
        StartCoroutine(FadeVCA(masterVCA, 0, 1, 2));

        if (fadeImage) yield return FadeScreen(Color.black, screenFadeDuration);
        SceneManager.LoadScene(creditsSceneName);
    }

    IEnumerator FadeVCA(VCA vca,float from,float to,float dur)
    {
        float t=0;
        while(t<dur){ t+=Time.deltaTime; vca.setVolume(Mathf.Lerp(from,to,t/dur)); yield return null; }
        vca.setVolume(to);
    }

    IEnumerator FadeScreen(Color target,float dur)
    {
        if (fadeImage==null) yield break;
        Color start = fadeImage.color; float t=0;
        while(t<dur){ t+=Time.deltaTime; fadeImage.color = Color.Lerp(start,target,t/dur); yield return null; }
        fadeImage.color = target;
    }

    IEnumerator FadeImage(Image img,float fromA,float toA,float dur)
    {
        if (!img) yield break;
        Color c = img.color; float t=0;
        while(t<dur){ t+=Time.deltaTime; c.a=Mathf.Lerp(fromA,toA,t/dur); img.color=c; yield return null; }
        c.a=toA; img.color=c;
        if (toA==0) img.gameObject.SetActive(false);
    }

    IEnumerator FadeText(TextMeshProUGUI txt,float fromA,float toA,float dur)
    {
        Color c=txt.color; float t=0;
        while(t<dur){ t+=Time.deltaTime; c.a=Mathf.Lerp(fromA,toA,t/dur); txt.color=c; yield return null; }
        c.a=toA; txt.color=c;
    }
}