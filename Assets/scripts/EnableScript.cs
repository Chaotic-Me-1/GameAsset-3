using UnityEngine;

// Script by OlafRT
// enables a script when the object is enabled
// then disables the object if the self destruct is set to true

public class EnableScript : MonoBehaviour
{
    public MonoBehaviour scriptToEnable;
    public bool selfDestruct = true;

    void OnEnable()
    {
        if (scriptToEnable != null)
        {
            scriptToEnable.enabled = true;
        }

        if (selfDestruct)
            enabled = false;
    }
}