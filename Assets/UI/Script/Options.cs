using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class Options : MonoBehaviour
{
    //Unity
    public Slider Mastersound; //Gets the slider for the volume controll
    public AudioMixer audioMixer; //Gets the audiomixer to controll sound
    [SerializeField] private GameObject OptionMenu; // takes the options 
    [SerializeField] private GameObject MainMenu; //takes the main menu or other menues such as the pause menu UI 
    [SerializeField] private Toggle fullscreenToggle; //connects to the toggel in the mneu to add fullscreen setting to

    //Fmod Bus
    public Bus masterBus;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);//To make sure the sound dont gets back to default to the next sceen 
        LoadVolume();

        //Get the main Fmod bus
        masterBus = RuntimeManager.GetBus("bus:/");
    }

    private void Start()
    {
        
        LoadFullscreen();
    }
    
    public void OpenOptionsMenu()
    {
        ///Hides the main menu design and shows the option
        OptionMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void OpenOptionsMenuClose()
    {
        //hides the option designs and return to the main menu
        OptionMenu.SetActive(false);
        MainMenu.SetActive(true);
    }


    public void SetVolume(float volume)
    {
        ///the currentVolume will be the value of the slider
        float currentVolume = Mastersound.value; 

        AudioListener.volume = currentVolume; //Unity's Master volume
        masterBus.setVolume(currentVolume); //Fmod Master Bus
        PlayerPrefs.SetFloat("soundvolume", currentVolume);
    }

    public void SaveVolume()
    {
        //Saves the sound value
        PlayerPrefs.SetFloat("soundVolume", Mastersound.value);
    }

    public void LoadVolume()
    {
        //Load saved volume, or use default 1 that is full volume
        float savedVolume = PlayerPrefs.GetFloat("soundVolume", 1);
        Mastersound.value = savedVolume;
        SetVolume(savedVolume);//Making sure that both volume gets updates 
    }

    //using an int to let it go from 0 to 2 to go between low, mid and high
    public void SetQuality (int qualityIndex)
    {
        //using the index that corrosonds to the project setting quality to change the quality of the game
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Set the game to fullscreen or windowed mode based on the value of the 'isFullscreen' boolean
    public void SetToFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        //saves the prefrence were 1 is fullscreen and 0 is windowed 
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0); 
        PlayerPrefs.Save();
        Debug.Log("Fullscreen button clicked"); //Sending a debut to see if the check boxs have been clicked 
    }

    // Load the fullscreen preference
    public void LoadFullscreen()
    {
        //Loade the saved default fullscreen thats 1
        bool savedFullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        Screen.fullScreen = savedFullscreen; //applies the fullscreen mode

        // If the fullscreen toggle exists in the scene, update its checked state to match the saved setting
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = savedFullscreen;
    }
    public void Update()
    {
        //when clicking escape the UI will desperate like the pause menu
        if (Input.GetKeyDown(KeyCode.Escape)&& OptionMenu.activeSelf)
        {
            OptionMenu.SetActive(false);
        }
    }
}
