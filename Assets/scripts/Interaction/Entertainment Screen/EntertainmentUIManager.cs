using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using FMODUnity;
using FMOD.Studio;
using TMPro;

public class EntertainmentUIManager : MonoBehaviour
{
    [Header("Main UI Panels")]
    public GameObject entertainmentPanel;
    public GameObject moviesPanel;
    public GameObject showsPanel;
    public GameObject musicPanel;
    public GameObject mediaPlayerPanel;

    [Header("Hand Cursor")]
    public Image handImage;
    public Sprite normalHandSprite;
    public Sprite clickHandSprite;
    public float clickDuration = 0.5f;

    [Header("Shared Controls")]
    public Button playPauseButton;
    public Sprite playIcon;
    public Sprite pauseIcon;
    private bool isPlaying = false;
    private bool isVideo = false;
    public Slider musicSlider;

    [Header("Music Player")]
    public Image albumArtDisplay;
    public TextMeshProUGUI trackNameText;

    private EventInstance musicInstance;
    private MusicTrackData currentTrack;

    [Header("Video Player")]
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;

    private EventInstance videoAudioInstance;

    [Header("Video Data Buttons")]
    public TextMeshProUGUI videoTitleText;

    private bool isSeeking = false;
    private bool wasPlayingBeforeSeek = false;
    
    void Start()
    {
        CloseAllTabs();
        entertainmentPanel.SetActive(false);
        mediaPlayerPanel.SetActive(false);
        musicSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        if (!isSeeking)
        {
            if (isVideo && videoPlayer.isPlaying && videoPlayer.length > 0)
            {
                double normalized = videoPlayer.time / videoPlayer.length;
                musicSlider.SetValueWithoutNotify((float)normalized);
            }
            else if (!isVideo && musicInstance.isValid())
            {
                musicInstance.getTimelinePosition(out int positionMS);
                musicInstance.getDescription(out var desc);
                desc.getLength(out int lengthMS);

                if (lengthMS > 0)
                {
                    float normalized = (float)positionMS / lengthMS;
                    musicSlider.SetValueWithoutNotify(normalized);
                }
            }
        }

        if (entertainmentPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            HideEntertainmentUI();

        if (mediaPlayerPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseMediaPlayer();
    }

    // ---------------- UI CONTROL ----------------

    public void ShowEntertainmentUI()
    {
        entertainmentPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void HideEntertainmentUI()
    {
        entertainmentPanel.SetActive(false);
        mediaPlayerPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void OpenMoviesTab()
    {
        CloseAllTabs();
        moviesPanel.SetActive(true);
    }

    public void OpenShowsTab()
    {
        CloseAllTabs();
        showsPanel.SetActive(true);
    }

    public void OpenMusicTab()
    {
        CloseAllTabs();
        musicPanel.SetActive(true);
    }

    void CloseAllTabs()
    {
        moviesPanel.SetActive(false);
        showsPanel.SetActive(false);
        musicPanel.SetActive(false);
    }

    public void AnimateHandClick()
    {
        if (handImage != null)
            StartCoroutine(ClickAnimation());
    }

    private IEnumerator ClickAnimation()
    {
        handImage.sprite = clickHandSprite;
        yield return new WaitForSecondsRealtime(clickDuration);
        handImage.sprite = normalHandSprite;
    }

    // ---------------- MUSIC ----------------

    public void PlayMusic(MusicTrackData track)
    {
        StopCurrentMedia();

        mediaPlayerPanel.SetActive(true);
        isVideo = false;

        albumArtDisplay.gameObject.SetActive(true);
        videoDisplay.gameObject.SetActive(false);

        trackNameText.text = track.trackName;
        albumArtDisplay.sprite = track.albumArt;

        musicInstance = RuntimeManager.CreateInstance(track.fmodEvent);
        RuntimeManager.AttachInstanceToGameObject(musicInstance, transform);
        musicInstance.start();

        isPlaying = true;
        UpdatePlayPauseIcon();
    }

    public void PlayMusicFromButton(MusicTrackData track)
    {
        // Only start the music if it’s not already playing this one
        if (currentTrack == track && musicInstance.isValid())
        {
            mediaPlayerPanel.SetActive(true); // Just reopen player for control
            return;
        }

        StopCurrentMedia(); // Stop any previously playing music/video

        currentTrack = track;

        trackNameText.text = track.trackName;
        albumArtDisplay.sprite = track.albumArt;

        mediaPlayerPanel.SetActive(true);
        isVideo = false;

        albumArtDisplay.gameObject.SetActive(true);
        videoDisplay.gameObject.SetActive(false);

        musicInstance = RuntimeManager.CreateInstance(track.fmodEvent);
        RuntimeManager.AttachInstanceToGameObject(musicInstance, transform);
        musicInstance.start();

        isPlaying = true;
        UpdatePlayPauseIcon();
    }

    // ---------------- VIDEO ----------------

    public void PlayMovie(VideoClip clip)
    {
        StopCurrentMedia();

        mediaPlayerPanel.SetActive(true);
        isVideo = true;

        albumArtDisplay.gameObject.SetActive(false);
        videoDisplay.gameObject.SetActive(true);

        videoPlayer.clip = clip;
        videoPlayer.Play();

        isPlaying = true;
        UpdatePlayPauseIcon();
    }

    // ---------------- SHARED CONTROLS ----------------

    public void TogglePlayPause()
    {
        if (isVideo)
        {
            if (isPlaying)
            {
                videoPlayer.Pause();
                if (videoAudioInstance.isValid()) videoAudioInstance.setPaused(true);
            }
            else
            {
                videoPlayer.Play();
                if (videoAudioInstance.isValid()) videoAudioInstance.setPaused(false);
            }
        }
        else
        {
            if (musicInstance.isValid())
            {
                musicInstance.setPaused(isPlaying);
            }
        }

        isPlaying = !isPlaying;
        UpdatePlayPauseIcon();
    }

    public void OnSliderValueChanged(float value)
    {
        if (isSeeking)
        {
            SeekInMedia(value); // only seek while dragging
        }
    }

    public void OnSliderPointerDown(BaseEventData eventData)
    {
        isSeeking = true;

        if (!isVideo && musicInstance.isValid())
        {
            musicInstance.getPaused(out bool paused);
            wasPlayingBeforeSeek = !paused;
        }
    }

    public void OnSliderPointerUp(BaseEventData eventData)
    {
        isSeeking = false;
        SeekInMedia(musicSlider.value);

        if (!isVideo && musicInstance.isValid() && wasPlayingBeforeSeek)
        {
            musicInstance.setPaused(false); // resume
        }
    }

    public void SeekInMedia(float value)
    {
        if (isVideo && videoPlayer.length > 0)
        {
            double newTime = value * videoPlayer.length;
            videoPlayer.time = newTime;
        }
        else if (!isVideo && musicInstance.isValid())
        {
            musicInstance.getDescription(out var desc);
            desc.getLength(out int lengthMS);

            int newTimeMS = Mathf.FloorToInt(value * lengthMS);
            musicInstance.setTimelinePosition(newTimeMS);
        }
    }

    public void CloseMediaPlayer()
    {
        if (isVideo && videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        mediaPlayerPanel.SetActive(false);
    }

    public void ShowMediaPlayer()
    {
        mediaPlayerPanel.SetActive(true);
        albumArtDisplay.gameObject.SetActive(!isVideo);
        videoDisplay.gameObject.SetActive(isVideo);

        // Optional: update the play/pause icon when reopening
        UpdatePlayPauseIcon();
    }

    void StopCurrentMedia()
    {
        if (isVideo)
        {
            videoPlayer.Stop();

            if (videoAudioInstance.isValid())
            {
                videoAudioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                videoAudioInstance.release();
                videoAudioInstance.clearHandle();
            }
        }
        else if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
            musicInstance.clearHandle();
        }

        isPlaying = false;
    }

    void UpdatePlayPauseIcon()
    {
        if (playPauseButton != null)
        {
            playPauseButton.image.sprite = isPlaying ? pauseIcon : playIcon;
        }
    }

    public void PlayVideoFromButton(VideoTrackData track)
    {
        StopCurrentMedia();

        isVideo = true;
        isPlaying = true;

        albumArtDisplay.gameObject.SetActive(false);
        videoDisplay.gameObject.SetActive(true);
        mediaPlayerPanel.SetActive(true);

        videoTitleText.text = track.title;

        videoDisplay.texture = videoPlayer.targetTexture;
        videoPlayer.Stop();

        // URL-based video playback
        if (!string.IsNullOrEmpty(track.videoURL))
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = track.videoURL;

            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            AudioSource videoAudioSource = videoPlayer.GetComponent<AudioSource>();
            if (videoAudioSource != null)
            {
                videoPlayer.SetTargetAudioSource(0, videoAudioSource);
                videoAudioSource.volume = 1f;
                videoAudioSource.playOnAwake = false;
            }

            videoPlayer.Prepare();
            StartCoroutine(PlayStreamedVideoWhenReady());
        }
        // Local clip + FMOD flow
        else if (track.clip != null)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = track.clip;
            videoPlayer.Play();

            if (!track.fmodAudio.IsNull)
            {
                videoAudioInstance = RuntimeManager.CreateInstance(track.fmodAudio);
                RuntimeManager.AttachInstanceToGameObject(videoAudioInstance, transform);
                videoAudioInstance.start();
            }
        }

        UpdatePlayPauseIcon();
    }

    private IEnumerator PlayStreamedVideoWhenReady()
    {
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        AudioSource videoAudioSource = videoPlayer.GetTargetAudioSource(0);
        if (videoAudioSource != null)
        {
            videoAudioSource.Play();
        }
    }
    
    public void ForceStopAllMedia()
    {
        StopCurrentMedia();
        trackNameText.text = "";
        currentTrack = null;
    }
}