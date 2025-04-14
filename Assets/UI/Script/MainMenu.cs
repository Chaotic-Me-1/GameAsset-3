using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("InFlight");
    }

    public void Quitgame()
    {
        Debug.Log("You are quiting the game");
        Application.Quit();
    }
}
