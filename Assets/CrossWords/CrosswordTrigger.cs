using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrosswordTrigger : MonoBehaviour, IPointerClickHandler
{
    public GameObject crosswordCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("📄 Crossword clicked!");
        crosswordCanvas.SetActive(true);
        Time.timeScale = 0f; // optional pause
    }
}

