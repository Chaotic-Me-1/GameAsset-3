using UnityEngine;
using TMPro;

public class DisplayPlayerName : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public TextMeshPro worldText;

    void Start()
    {
        if (PlayerNameManager.instance != null)
        {
            string playerName = PlayerNameManager.instance.GetPlayerName();
            string finalText = "Captain " + playerName;

            if (uiText != null)
                uiText.text = finalText;

            if (worldText != null)
                worldText.text = finalText;
        }
        else
        {
            Debug.LogWarning("PlayerNameManager instance not found.");
        }
    }
}