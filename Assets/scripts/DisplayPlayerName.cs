using UnityEngine;
using TMPro;

// Script by OlafRT
// We're getting the players name from the playernamemanager and using this
// script to display the name on a textmeshpro.

public class DisplayPlayerName : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public TextMeshPro worldText;

    void Start()
    {
        if (PlayerNameManager.instance != null)
        {
            string playerName = PlayerNameManager.instance.GetPlayerName();
            string finalText = playerName;

            if (uiText != null)
                uiText.text = finalText;

            if (worldText != null)
                worldText.text = finalText;
        }
        else
        {
            Debug.LogWarning("PlayerNameManager doesnt exist!!!.");
        }
    }
}