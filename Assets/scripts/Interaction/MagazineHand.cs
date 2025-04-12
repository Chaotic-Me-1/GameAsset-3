using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Assign this script to a part of the UI of the magazine, it will use an image component to overlay a hand sprite over the mouse cursor. Might be a better way to do this than using Update, I'm not sure. - Olaf
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

        // Update hand sprite based on if mouse is being held
        if (Input.GetMouseButton(0))
        {
            handImage.sprite = holdingHandSprite;
        }
        else
        {
            handImage.sprite = regularHandSprite;
        }
    }
}
