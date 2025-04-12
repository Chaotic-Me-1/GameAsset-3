using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineHand : MonoBehaviour
{
    [Header("Hand Sprite")]
    [Tooltip("The hand image (component) that follows the mouse cursor")]
    public Image handImage;
    [Tooltip("Default hand sprite")]
    public Sprite regularHandSprite;
    [Tooltip("Sprite used while mouse button held")]
    public Sprite holdingHandSprite;

    void Update()
    {
        // Force cursor hidden every frame
        Cursor.visible = false;

        if (handImage == null || !handImage.gameObject.activeInHierarchy)
            return;

        // Move hand image to follow cursor
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.canvas.transform as RectTransform,
            Input.mousePosition,
            handImage.canvas.worldCamera,
            out pos
        );
        handImage.rectTransform.anchoredPosition = pos;

        // Update hand sprite based on mouse state
        handImage.sprite = Input.GetMouseButton(0) ? holdingHandSprite : regularHandSprite;
    }
}
