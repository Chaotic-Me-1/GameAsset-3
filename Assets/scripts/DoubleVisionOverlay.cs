using UnityEngine;
using UnityEngine.UI;

// Script by OlafRT
// Used to make you seem really drunk in the game whenever you drink something strong.
// The intensity of this is increased by how strong the drink is, which we get from DrunknessManager.

public class DoubleVisionOverlay : MonoBehaviour
{
    public RawImage overlayImage;
    public float maxOffset = 10f; // how far it shifts at 100% drunk
    public float maxAlpha = 0.3f; // how transparent the render texture / raw image from the other camera is

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