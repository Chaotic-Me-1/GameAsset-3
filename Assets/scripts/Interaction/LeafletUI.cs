using UnityEngine;
using UnityEngine.UI;

public class LeafletUI : MonoBehaviour
{
    public GameObject leafletPanel;

    void Start()
    {
        leafletPanel.SetActive(false);
    }

    public void ShowLeaflet()
    {
        leafletPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Optional: pause game while reading
    }

    public void HideLeaflet()
    {
        leafletPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}

