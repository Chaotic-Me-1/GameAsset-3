using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script by Ragnar
// This script makes it possible to click on the magazine to open the crossword
public class CrosswordHotspot : MonoBehaviour, IPointerClickHandler
{
    [Header("Reference to the crossword canvas UI")]
    public GameObject crosswordCanvas;

    [Header("Optional: Lock interaction with book while solving")]
    public Book bookReference;

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowCrossword();
    }

    public void ShowCrossword()
    {
        if (crosswordCanvas != null)
        {
            crosswordCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }

        if (bookReference != null)
            bookReference.interactable = false;
    }

    public MagazineUI magazineUI;

    public void HideCrossword()
    {
        if (crosswordCanvas != null)
        {
            crosswordCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }

        if (bookReference != null)
            bookReference.interactable = true;

        if (magazineUI != null)
            magazineUI.ResumeMagazine(); 
    }
}
