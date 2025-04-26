using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseMenuCanvas;
    public string mainMenuSceneName;

    [Header("Block Pause If These Are Active")]
    public GameObject magazineUI;
    public GameObject entertainmentUI;
    public GameObject leafletUI;
    public GameObject extraUI;

    private bool isPaused = false;
    private List<AudioSource> allAudioSources = new List<AudioSource>();
    private ParticleSystem[] allParticleSystems;
    private VideoPlayer[] allVideoPlayers;
    private Dictionary<VideoPlayer, bool> videoPlayerPlayStates = new Dictionary<VideoPlayer, bool>();

    private Bus masterBus;

    void Start()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);

        allAudioSources.AddRange(FindObjectsOfType<AudioSource>());
        allParticleSystems = FindObjectsOfType<ParticleSystem>();
        allVideoPlayers = FindObjectsOfType<VideoPlayer>();

        foreach (var videoPlayer in allVideoPlayers)
            videoPlayerPlayStates[videoPlayer] = false;

        masterBus = RuntimeManager.GetBus("bus:/");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Don't pause if any of these UIs are open
            if ((magazineUI != null && magazineUI.activeSelf) ||
                (entertainmentUI != null && entertainmentUI.activeSelf) ||
                (leafletUI != null && leafletUI.activeSelf) ||
                (extraUI != null && extraUI.activeSelf))
            {
                return;
            }

            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        allAudioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
        allParticleSystems = FindObjectsOfType<ParticleSystem>();
        allVideoPlayers = FindObjectsOfType<VideoPlayer>();

        PauseOrResumeAudio(isPaused);
        PauseOrResumeParticles(isPaused);
        PauseOrResumeVideos(isPaused);

        masterBus.setPaused(isPaused);

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(isPaused);
    }

    void PauseOrResumeAudio(bool pause)
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
            {
                if (pause) audioSource.Pause();
                else audioSource.UnPause();
            }
        }
    }

    void PauseOrResumeParticles(bool pause)
    {
        foreach (ParticleSystem particleSystem in allParticleSystems)
        {
            if (pause) particleSystem.Pause();
            else particleSystem.Play();
        }
    }

    void PauseOrResumeVideos(bool pause)
    {
        foreach (VideoPlayer videoPlayer in allVideoPlayers)
        {
            if (pause)
            {
                videoPlayerPlayStates[videoPlayer] = videoPlayer.isPlaying;
                videoPlayer.Pause();
            }
            else
            {
                if (videoPlayerPlayStates.ContainsKey(videoPlayer) && videoPlayerPlayStates[videoPlayer])
                    videoPlayer.Play();
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogWarning("Main menu scene name not set!");
        }
    }
}