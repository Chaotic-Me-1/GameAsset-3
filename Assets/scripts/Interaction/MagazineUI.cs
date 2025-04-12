using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MagazineUI : MonoBehaviour
{
    public GameObject magazinePanel;
    public Book magazineBook; // Reference to the Book script

    [Header("Audio")]
    public EventReference openMagazineEvent;

    void Start()
    {
        magazinePanel.SetActive(false);
    }

    public void ShowMagazine()
    {
        magazinePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        Time.timeScale = 0f;

        if (magazineBook != null)
        {
            magazineBook.gameObject.SetActive(true);
            magazineBook.currentPage = 0;
            magazineBook.UpdateSprites(); // Assuming this refreshes page visuals
        }

        if (!openMagazineEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(openMagazineEvent);
        }
    }

    public void HideMagazine()
    {
        magazinePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }
}
