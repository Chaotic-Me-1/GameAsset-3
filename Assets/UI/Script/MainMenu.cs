using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Name Input UI")]
    public GameObject namePromptUI;          // The panel that holds the input field + prompt
    public TMP_InputField nameInputField;    // TMP input field
    public string sceneToLoad = "InFlight";  // Scene name to load after name is entered

    private bool isPromptActive = false;

    public void PlayGame()
    {
        namePromptUI.SetActive(true);
        nameInputField.text = "";
        nameInputField.Select();
        isPromptActive = true;
    }

    public void Quitgame()
    {
        Debug.Log("You are quitting the game");
        Application.Quit();
    }

    void Update()
    {
        if (isPromptActive && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitName();
        }
    }

    public void SubmitName()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            // Save it using the singleton
            if (PlayerNameManager.instance != null)
            {
                PlayerNameManager.instance.SetPlayerName(playerName);
            }

            // Load the game
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Player name cannot be empty!");
        }
    }
}
