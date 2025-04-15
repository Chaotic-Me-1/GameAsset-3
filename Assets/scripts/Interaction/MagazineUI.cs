using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MagazineUI : MonoBehaviour
{
    public GameObject magazinePanel;

    [Header("Audio")]
    public EventReference openMagazineEvent;

    void Start()
    {
        magazinePanel.SetActive(false);
    }

    void Update()
    {
        if (magazinePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HideMagazine();
        }
    }
    public void ResumeMagazine()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        magazinePanel.SetActive(true);
    }

    public void ShowMagazine()
    {
        magazinePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameState.OpenMagazine();

        if (!openMagazineEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(openMagazineEvent);
        }
    }

    public void HideMagazine()
    {
        magazinePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameState.CloseMagazine();
    }
}
