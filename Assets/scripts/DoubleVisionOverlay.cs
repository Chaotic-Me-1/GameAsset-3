using UnityEngine;
using UnityEngine.UI;

public class DoubleVisionOverlay : MonoBehaviour
{
    public RawImage overlayImage;
    public float maxOffset = 10f; // how far it shifts at 100% drunk
    public float maxAlpha = 0.3f;

    private RectTransform rectTransform;

    void Start()
    {
        if (overlayImage != null)
            rectTransform = overlayImage.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (DrunknessManager.instance == null || overlayImage == null) return;

        float drunkPercent = DrunknessManager.instance.currentDrunkness / 100f;

        // Fade in alpha
        Color color = overlayImage.color;
        color.a = Mathf.Lerp(0f, maxAlpha, drunkPercent);
        overlayImage.color = color;

        // Slight horizontal and vertical offset
        float offset = Mathf.Lerp(0f, maxOffset, drunkPercent);
        rectTransform.anchoredPosition = new Vector2(offset, offset);
    }
}