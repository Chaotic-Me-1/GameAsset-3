using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class LeafletUI : MonoBehaviour
{
    public GameObject leafletPanel;

    [Header("Audio")]
    public EventReference openLeafletEvent;  // Replaces [EventRef] string

    void Start()
    {
        leafletPanel.SetActive(false);
    }

    public void ShowLeaflet()
    {
        leafletPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        // Play FMOD sound with new EventReference
        if (openLeafletEvent.IsNull == false)
        {
            RuntimeManager.PlayOneShot(openLeafletEvent);
        }
    }

    public void HideLeaflet()
    {
        leafletPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}


