using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snoring : MonoBehaviour
{

    public Sound softSnoring;
    public Sound mediumSnoring;
    public Sound hardSnoring;
    public int randomsnoreNb;
    int Ssnore;
    // Start is called before the first frame update

    void Start()
    {
        Ssnore = 0;
        StartCoroutine(snoring());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator snoring()
    {
        yield return new WaitForSeconds(30);
       
        
       randomsnoreNb = Random.Range(0, 5);
        switch (randomsnoreNb)
        {
            case 0:
                    UnityEngine.Debug.Log("Snoring 0");
                break;
            case 1:

                    UnityEngine.Debug.Log("Snoring 0");
                break;
            case 2:
                    UnityEngine.Debug.Log("Snoring 0");
                break;
            default:
                    UnityEngine.Debug.Log("not snoring");
            break;
        }
        if(Ssnore == 3)
        {
            StartCoroutine(Spesialsnore());
            StopCoroutine(snoring());

        }

        Ssnore++;
    }
    IEnumerator Spesialsnore()
    {
        int randomSsnoreNb = Random.Range(0, 1);
        switch (randomSsnoreNb)
        {
            case 0:
                UnityEngine.Debug.Log("Snoring 0");
                break;
            case 1:
                UnityEngine.Debug.Log("Snoring 1");
                break;
            default :
            break;

        }

        yield return new WaitForSeconds(30);
    }
        

    
    public void countdown()
    {
        

    }
   
}
