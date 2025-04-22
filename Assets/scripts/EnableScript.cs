using UnityEngine;

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
