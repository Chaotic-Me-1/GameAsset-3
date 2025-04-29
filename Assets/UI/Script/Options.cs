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

    //Fmod
  //  public Slider MusicSlider;
  //  public Slider SFXSlider;
  //  private bus musicBus;
  //  private bus sfxBus;

    private void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
            LoadVolume();

        else
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
            LoadVolume();
        }
    }
    
    public void OpenOptionsMenu()
    {
        OptionMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void OpenOptionsMenuClose()
    {
        OptionMenu.SetActive(false);
        MainMenu.SetActive(true);
    }


    public void SetVolume(float volume)
    {
        AudioListener.volume = Mastersound.value;
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", Mastersound.value);
    }

    public void LoadVolume()
    {
        Mastersound.value = PlayerPrefs.GetFloat("soundVolume");
    }

    //using an int to let it go from 0 to 2 to go between low, mid and high
    public void SetQuality (int qualityIndex)
    {
        //using the index that corrosonds to the project setting quality to change the quality of the game
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetToFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen button clicked");
    }

    //public void SetMusicVolume (float volume)
   // {
        //musicBus.setVolume(volume);
        //PlayerPrefs.SetFloat("MusicVolume,")
   // }
}
