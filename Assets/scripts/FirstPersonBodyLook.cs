using UnityEngine;

public class FirstPersonBodyLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform spineJoint;
    public Transform neckJoint;
    public Transform headJoint;

    private float xRotation = 0f;
    private float yRotation = 0f;

    [Header("Drunk Wobble Settings")]
    public float maxWobbleAngle = 10f;   // Max sway angle in degrees
    public float wobbleSpeed = 2f;       // Speed of swaying
    private float wobbleTimer = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Freeze look during dialogue
        if (DialogueManager.IsDialogueActive)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        // Drunk wobble
        float drunkLevel = DrunknessManager.instance != null ? DrunknessManager.instance.currentDrunkness / 100f : 0f;

        float wobbleX = 0f;
        float wobbleZ = 0f;

        if (drunkLevel > 0.01f)
        {
            wobbleTimer += Time.deltaTime * wobbleSpeed * (0.5f + drunkLevel);
            wobbleX = Mathf.Sin(wobbleTimer * 1.1f) * maxWobbleAngle * drunkLevel;
            wobbleZ = Mathf.Cos(wobbleTimer) * maxWobbleAngle * drunkLevel;
        }

        // Apply rotation
        spineJoint.localRotation = Quaternion.Euler(wobbleX * 0.2f, yRotation * 0.3f + wobbleZ * 0.2f, 0f);
        neckJoint.localRotation = Quaternion.Euler(wobbleX * 0.4f, yRotation * 0.7f + wobbleZ * 0.4f, 0f);
        headJoint.localRotation = Quaternion.Euler(xRotation + wobbleX, 0f, wobbleZ);
    }
}