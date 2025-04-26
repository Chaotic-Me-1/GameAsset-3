using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MagazineUI : MonoBehaviour
{
    public GameObject magazinePanel;
    public GameObject extraPanelToClose;

    [Header("Audio")]
    public EventReference openMagazineEvent;

    void Start()
    {
        magazinePanel.SetActive(false);
        if (extraPanelToClose != null)
            extraPanelToClose.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool anyClosed = false;

            if (magazinePanel.activeSelf)
            {
                HideMagazine();
                anyClosed = true;
            }

            if (extraPanelToClose != null && extraPanelToClose.activeSelf)
            {
                extraPanelToClose.SetActive(false);
                anyClosed = true;
            }

            if (anyClosed)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
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
        GameState.CloseMagazine();
    }
}