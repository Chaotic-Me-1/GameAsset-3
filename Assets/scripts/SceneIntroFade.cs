using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class SceneIntroFade : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public Volume globalVolume;  
    public float fadeDuration = 2f;

    [Header("FMOD Sound")]
    public EventReference introSound;

    private Vignette vignette;

    void Start()
    {
        if (fadeImage != null)
        {
            // Start the screen as black to fade it in
            fadeImage.color = Color.black;
            fadeImage.gameObject.SetActive(true);
        }

        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out vignette);

            if (vignette != null)
            {
                vignette.active = true;
                vignette.intensity.overrideState = true;
                vignette.smoothness.overrideState = true;

                vignette.intensity.value = 1f;
                vignette.smoothness.value = 1f;
            }
        }

        if (!introSound.IsNull)
        {
            EventInstance sfx = RuntimeManager.CreateInstance(introSound);
            RuntimeManager.AttachInstanceToGameObject(sfx, transform);
            sfx.start();
            sfx.release();
        }
        
        StartCoroutine(FadeInSequence());
    }

    private IEnumerator FadeInSequence()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            float eased = EaseOutCubic(t);

            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, 1f - eased);
            }

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(1f, 0.32f, eased);
                vignette.smoothness.value = Mathf.Lerp(1f, 0.32f, eased);
            }

            yield return null;
        }

        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false); // when fully faded, disable the UI image

        if (vignette != null)
        {
            vignette.intensity.value = 0.32f;
            vignette.smoothness.value = 0.32f;
        }
    }


    private float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
}
