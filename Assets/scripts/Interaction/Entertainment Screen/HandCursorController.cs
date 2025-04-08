using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCursorController : MonoBehaviour
{
    public Image handImage;
    public EntertainmentUIManager uiManager;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // 🔥 Force cursor to remain hidden every frame (especially after UI clicks)
        Cursor.visible = false;

        if (handImage == null || !handImage.gameObject.activeInHierarchy)
            return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.canvas.transform as RectTransform,
            Input.mousePosition,
            handImage.canvas.worldCamera,
            out pos
        );

        handImage.rectTransform.anchoredPosition = pos;

        if (Input.GetMouseButtonDown(0))
        {
            uiManager?.AnimateHandClick();
        }
    }

    void OnDisable()
    {
        // 🟢 Optional: Restore system cursor when leaving this UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}