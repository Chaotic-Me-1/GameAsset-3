using UnityEngine;
using FMODUnity;

public class JumpScare : MonoBehaviour
{
    [Header("UI Image or Sprite Renderer, only one at a time pls")]
    public UnityEngine.UI.Image uiImage;
    public SpriteRenderer       worldSprite;

    [Header("Timing")]
    [Min(0)] public float delayBeforeScare = 5f;
    [Min(1)] public int   flashes          = 3;
    public float flashOnTime  = 0.5f;
    public float flashOffTime = 0.5f;

    [Header("Scale growth")]
    [Tooltip("How much thats added to scale EVERY flash")]
    public float sizeIncrementPerFlash = 0.5f;

    [Header("Sound")]
    public EventReference scareSound;

    Transform targetTf;
    Vector3   baseScale;

    void OnEnable()
    {
        // decide which renderer we’re using
        if (uiImage != null)      targetTf = uiImage.rectTransform;
        else if (worldSprite != null) targetTf = worldSprite.transform;
        else
        {
            Debug.LogWarning($"{name}: No UI Image or SpriteRenderer!");
            enabled = false;
            return;
        }

        baseScale = targetTf.localScale;
        SetVisible(false);
        StartCoroutine(ScareRoutine());
    }

    System.Collections.IEnumerator ScareRoutine()
    {
        yield return new WaitForSeconds(delayBeforeScare);

        for (int i = 0; i < flashes; i++)
        {
            float scaleFactor = 1f + sizeIncrementPerFlash * i;
            targetTf.localScale = baseScale * scaleFactor;

            SetVisible(true);
            PlayScareSound();

            yield return new WaitForSeconds(flashOnTime);

            SetVisible(false);
            yield return new WaitForSeconds(flashOffTime);
        }

        targetTf.localScale = baseScale;
        SetVisible(false);
    }

    void SetVisible(bool v)
    {
        if (uiImage      != null) uiImage.enabled        = v;
        if (worldSprite  != null) worldSprite.enabled    = v;
    }

    void PlayScareSound()
    {
        if (!scareSound.IsNull)
            RuntimeManager.PlayOneShot(scareSound);
    }
}