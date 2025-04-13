using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phsound : MonoBehaviour
{
    public int numbergen;
    public int timer;
    public int trigger;
    public SoundCooldown SoundCooldown;
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
        if (numbergen == 0 && SoundCooldown.count == false)
        {
            //playsound
            SoundCooldown.Cooldown();
            StopCoroutine(Phevent());
        }
        else
        {
            numbergen = -1; 


        }
    
    }
}
