using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Snoring : MonoBehaviour
{
    [Header("FMOD Snoring Event")]
    
    public EventReference softSnoring;
    public EventReference mediumSnoring;
    public EventReference hardSnoring;
    public EventReference deathSnoring;
    public EventReference wakeSnoring;
    private int randomsnoreNb;
    int Ssnore;
    // Start is called before the first frame update

    void Start()
    {
        
        Ssnore = 0;
        
    }

    // Update is called once per frame
   
    IEnumerator snoring() // Idea is to have some random snoring sounds that can happen
    {
        yield return new WaitForSeconds(3);
       
        
       randomsnoreNb = Random.Range(0, 5);
        switch (randomsnoreNb)
        {
            case 0:
                PlaySound(softSnoring);
                    Debug.Log("Snoring 0");
                break;
            case 1:

                    Debug.Log("Snoring 0");
                break;
            case 2:
                    Debug.Log("Snoring 0");
                break;
            default:
                    Debug.Log("not snoring");
            break;
        }
        if(Ssnore == 3)
        {
            StartCoroutine(Spesialsnore());
            Ssnore = 0;
            StopCoroutine(snoring());
           

        }

        Ssnore++;
        StartCoroutine(snoring());
    }
    IEnumerator Spesialsnore() // after 3 normal snoring sounds there will be kinda of a Special sound that happends and that can be random as well 
    {
        int randomSsnoreNb = Random.Range(0, 1);
        switch (randomSsnoreNb)
        {
            case 0:
                Debug.Log("Snoring 0");
                break;
            case 1:
                Debug.Log("Snoring 1");
                break;
            default :
            break;

        }

        yield return new WaitForSeconds(30);
    }
    private void snore()
    { 
        StartCoroutine(snoring());
    }
    private void PlaySound(EventReference soundEvent) //plays the sound that is meant to be playing at the transform position of the gameobject with this script on it.
    {
        if (soundEvent.IsNull) return; //if there isnt a fmod sound event assigned we skip this so we don't get a gazillion errors.

        RuntimeManager.PlayOneShot(soundEvent, transform.position);


    }



}
