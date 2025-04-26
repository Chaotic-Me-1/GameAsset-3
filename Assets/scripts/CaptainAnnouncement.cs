using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// Here we are changing the broadcast sound depending on what loop we are on
// we are checking the LoopCycleManager script to see what loop we are on, 
// and then playing the fmod event made for that loop!

public class CaptainAnnouncement : MonoBehaviour
{
    [Header("Captain Announcements by Loop")]
    public EventReference loop0Announcement;
    public EventReference loop1Announcement;
    public EventReference loop2Announcement;
    public EventReference loop3Announcement;
    public EventReference loop4Announcement;
    public EventReference fallbackAnnouncement;

    void Start()
    {
        int loop = LoopCycleManager.instance != null ? LoopCycleManager.instance.loopCount : 0;
        EventReference selectedEvent = GetAnnouncementForLoop(loop);

        if (!selectedEvent.IsNull)
        {
            EventInstance instance = RuntimeManager.CreateInstance(selectedEvent);
            RuntimeManager.AttachInstanceToGameObject(instance, transform);
            instance.start();
            instance.release();
        }
    }

    private EventReference GetAnnouncementForLoop(int loop)
    {
        switch (loop)
        {
            case 0: return loop0Announcement;
            case 1: return loop1Announcement;
            case 2: return loop2Announcement;
            case 3: return loop3Announcement;
            case 4: return loop4Announcement;
            default: return fallbackAnnouncement;
        }
    }
}