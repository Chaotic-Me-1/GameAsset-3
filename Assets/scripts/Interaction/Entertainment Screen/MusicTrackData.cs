using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "Entertainment/MusicTrackData")]
public class MusicTrackData : ScriptableObject
{
    public string trackName;
    public Sprite albumArt;
    public EventReference fmodEvent;
}