using UnityEngine;

public class CursorActivator : MonoBehaviour
{
    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Cursor enabled via animation event.");
    }
}