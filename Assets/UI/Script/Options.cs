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
    public Slider Mastersound;
    public AudioMixer audioMixer;
    [SerializeField] private GameObject OptionMenu;
    [SerializeField] private GameObject MainMenu;

    //Fmod Bus
    public Bus masterBus;

    private void Start()
    {
        //Get the main Fmod bus
        masterBus = RuntimeManager.GetBus("bus:/");

        LoadVolume();
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
        float currentVolume = Mastersound.value;

        AudioListener.volume = currentVolume; //Unity's Master volume
        masterBus.setVolume(currentVolume); //Fmod Master Bus
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
        SetVolume(Mastersound.value);//Making sure that both volume gets updates 
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
        Debug.Log("Fullscreen button clicked"); //Sending a debut to see if the check boxs have been clicked 
    }

}
