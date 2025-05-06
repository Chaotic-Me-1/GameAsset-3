using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Optionpart2 : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        // Load fullscreen preference when the scene starts
        LoadFullscreen();
    }

    //using an int to let it go from 0 to 2 to go between low, mid and high
    public void SetQuality(int qualityIndex)
    {
        //using the index that corrosonds to the project setting quality to change the quality of the game
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Set the game to fullscreen or windowed mode based on the value of the 'isFullscreen' boolean
    public void SetToFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0); // Use "fullscreen" consistently
        PlayerPrefs.Save();
        Debug.Log("Fullscreen button clicked"); //Sending a debut to see if the check boxs have been clicked 
    }

    // Load the fullscreen preference
    public void LoadFullscreen()
    {
        // Use the same key "fullscreen" to match the saved value
        bool savedFullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        Screen.fullScreen = savedFullscreen;

        // Sync the toggle if it's assigned
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreen;
    }
}