using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class EventSounds : MonoBehaviour
{
    [Header("FMOD Events")]
    [SerializeField] private EventReference stepSound;
    [SerializeField] private EventReference talkSound;
    [SerializeField] private EventReference serveSound;
    [SerializeField] private Animator attendent;


    //Playing sounds with Animation Events, we set these up in the animation that will trigger the sound. 
    //For example "Step" would be triggered by the animation event "Step" whenever the flight attendant takes a step.

    
    public void Step() //footstep
    {
        
            PlaySound(stepSound);
    }

    public void Talk() //talking
    {
            PlaySound(talkSound);
    }

    public void Serve() //serving food
    {
            PlaySound(serveSound);
    }

    private void PlaySound(EventReference soundEvent) //plays the sound that is meant to be playing at the transform position of the gameobject with this script on it.
    {
        if (soundEvent.IsNull) return; //if there isnt a fmod sound event assigned we skip this so we don't get a gazillion errors.

        RuntimeManager.PlayOneShot(soundEvent, transform.position);
        
        
    }
    
}
