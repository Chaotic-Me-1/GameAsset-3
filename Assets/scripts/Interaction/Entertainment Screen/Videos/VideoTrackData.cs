using UnityEngine;
using UnityEngine.Video;
using FMODUnity;

[CreateAssetMenu(fileName = "VideoTrackData", menuName = "Entertainment/VideoTrack")]
public class VideoTrackData : ScriptableObject
{
    public string title;
    public Sprite thumbnail;
    public VideoClip clip;
    public string videoURL;
    public FMODUnity.EventReference fmodAudio;
}