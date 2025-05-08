using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script by OlafRT
// Overlays a hand image on top of the cursor, where the pivot point of the image should be set to the
// point where you want the cursor to be. In this case that would mean the tip of the index finger.

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
        // Force cursor to remain hidden every frame
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}