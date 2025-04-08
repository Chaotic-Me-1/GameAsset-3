using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntertainmentUIManager : MonoBehaviour
{
    public GameObject entertainmentPanel;
    public GameObject moviesPanel;
    public GameObject showsPanel;
    public GameObject musicPanel;

    public Image handImage;
    public Sprite normalHandSprite;
    public Sprite clickHandSprite;
    public float clickDuration = 0.5f;

    void Start()
    {
        CloseAllTabs();
        entertainmentPanel.SetActive(false);
    }

    public void ShowEntertainmentUI()
    {
        entertainmentPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void HideEntertainmentUI()
    {
        entertainmentPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void OpenMoviesTab()
    {
        CloseAllTabs();
        moviesPanel.SetActive(true);
    }

    public void OpenShowsTab()
    {
        CloseAllTabs();
        showsPanel.SetActive(true);
    }

    public void OpenMusicTab()
    {
        CloseAllTabs();
        musicPanel.SetActive(true);
    }

    void CloseAllTabs()
    {
        moviesPanel.SetActive(false);
        showsPanel.SetActive(false);
        musicPanel.SetActive(false);
    }

    public void AnimateHandClick()
    {
        if (handImage != null && clickHandSprite != null && normalHandSprite != null)
            StartCoroutine(ClickAnimation());
    }

    private System.Collections.IEnumerator ClickAnimation()
    {
        handImage.sprite = clickHandSprite;
        yield return new WaitForSecondsRealtime(clickDuration);
        handImage.sprite = normalHandSprite;
    }
}
