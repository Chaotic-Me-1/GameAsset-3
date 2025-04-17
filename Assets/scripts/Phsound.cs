using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phsound : MonoBehaviour
{
    public int numbergen;
    public int timer;
    public int trigger;
    public SoundCooldown SoundCooldown;
    [SerializeField] private EventReference Punch;
    [SerializeField] private EventReference Slap;
    // Start is called before the first frame update
    void Start()
    {
        timer = 30;
        numbergen = 120;
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Phevent()
    {
        yield return new WaitForSeconds(timer);

        trigger = Random.Range(0, numbergen);
       
        if ((numbergen == 0 || numbergen == 50) && SoundCooldown.count == false   )
        {
            switch (trigger)
            { 
            case 0:

               RuntimeManager.CreateInstance(Punch);
                    break;

            case 50:

                    RuntimeManager.CreateInstance(Slap);
                    break;

            default:
                    Debug.Log("");
                        break;
            }
                
            
            SoundCooldown.Cooldown();
            StopCoroutine(Phevent());
        }
        else
        {
            numbergen = -1; 


        }
    
    }
}
