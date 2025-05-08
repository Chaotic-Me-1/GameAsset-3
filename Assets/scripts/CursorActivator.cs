using UnityEngine;

// Script by OlafRT
// Very simple, we use an animation event to enable the cursor
// this is used on the credits scene when the buttons pop up.

public class CursorActivator : MonoBehaviour
{
    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Cursor enabled from animation event.");
    }
}