using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToCredits : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;

    [Header("Timing")]
    public float delayBeforeFade = 2f;
    public float fadeDuration    = 3f; 

    [Header("Scene")]
    public string creditsSceneName = "Credits";

    void OnEnable()
    {
        if (fadeImage == null)
        {
            return;
        }

        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, 0f);

        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, k);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
        SceneManager.LoadScene(creditsSceneName);
    }
}