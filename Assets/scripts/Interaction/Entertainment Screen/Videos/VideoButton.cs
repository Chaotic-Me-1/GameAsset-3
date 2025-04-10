using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoButton : MonoBehaviour
{
    public VideoTrackData videoTrack;
    public EntertainmentUIManager uiManager;

    public void Play()
    {
        if (videoTrack != null && uiManager != null)
            uiManager.PlayVideoFromButton(videoTrack);
    }
}
