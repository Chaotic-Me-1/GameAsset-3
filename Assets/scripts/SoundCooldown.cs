using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCooldown : MonoBehaviour
{

    public bool count;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Cooldown()
    {

        count = true;

        StartCoroutine(countdown());


    }
    IEnumerator countdown()
    {
        yield return new WaitForSeconds(30);
        count = false;

    }
}
