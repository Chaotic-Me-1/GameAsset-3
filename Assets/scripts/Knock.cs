using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Knock : MonoBehaviour
{
    public EventReference smallTaps;
    public EventReference mediumTaps;
    public EventReference loudTaps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PlaySound(EventReference soundEvent) //plays the sound that is meant to be playing at the transform position of the gameobject with this script on it.
    {
        if (soundEvent.IsNull) return; //if there isnt a fmod sound event assigned we skip this so we don't get a gazillion errors.

        RuntimeManager.PlayOneShot(soundEvent, transform.position);


    }
}
