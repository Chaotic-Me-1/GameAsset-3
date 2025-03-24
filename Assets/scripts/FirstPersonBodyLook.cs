using UnityEngine;

public class FirstPersonBodyLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform spineJoint;
    public Transform neckJoint;
    public Transform headJoint;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()  // Was Update(), now LateUpdate
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        spineJoint.localRotation = Quaternion.Euler(0f, yRotation * 0.3f, 0f);
        neckJoint.localRotation = Quaternion.Euler(0f, yRotation * 0.7f, 0f);
        headJoint.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
