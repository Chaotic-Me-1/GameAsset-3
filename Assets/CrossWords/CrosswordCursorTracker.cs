using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosswordCursorTracker : MonoBehaviour
{
    public RectTransform handCursor; // Assign the CrosswordHand image here
    public Canvas canvas;             // Reference to the canvas

    public static CrosswordCursorTracker Instance;

    void Awake()
    {
        Instance = this;
        if (handCursor != null)
            handCursor.gameObject.SetActive(false);
    }

    public void MoveToCell(RectTransform cellRectTransform)
    {
        if (handCursor == null || canvas == null || cellRectTransform == null) return;

        Vector3[] corners = new Vector3[4];
        cellRectTransform.GetWorldCorners(corners);
        Vector3 worldCenter = (corners[0] + corners[2]) / 2f;

        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            worldCenter,
            canvas.worldCamera,
            out anchoredPosition);

        Debug.Log("Moving cursor to: " + anchoredPosition);


        handCursor.anchoredPosition = anchoredPosition;
        handCursor.gameObject.SetActive(true);
    }

    public void HideCursor()
    {
        if (handCursor != null)
            handCursor.gameObject.SetActive(false);
    }
}