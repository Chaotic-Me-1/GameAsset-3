using UnityEngine;
using UnityEngine.Video;
using FMODUnity;

[CreateAssetMenu(fileName = "NewVideoTrack", menuName = "Entertainment/Video Track")]
public class VideoTrackData : ScriptableObject
{
    public string title;
    public Sprite thumbnail;
    public VideoClip clip;
    public EventReference fmodAudio;
}