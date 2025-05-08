using UnityEngine;

// Script by OlafRT
// the scales of Ma'at (truth)
// this script adjusts the scales depending on your current amount of karma
// by adjusting the rotation of the arm and the height (Y) poistion of the bowls.

public class KarmaScaleController : MonoBehaviour
{
    [Header("References")]
    public Transform arm; // The rotating arm object
    public Transform leftBowl; // Bowl holding the feather
    public Transform rightBowl; // Bowl holding the heart

    [Header("Karma Settings")]
    [Range(0, 100)]
    public int karmaPoints = 50; // Default neutral
    private float targetZRotation; // Target rotation for the arm

    [Header("Bowl Vertical Movement")]
    public float defaultBowlY = 1f; // Default Y position for bowls at neutral
    public float bowlYOffset = 1f;  // Max Y offset (+1/-1)

    void Update()
    {
        // Get current karma from KarmaManager
        if (KarmaManager.instance != null)
            karmaPoints = KarmaManager.instance.karmaPoints;

        // Adjust karma (0–100) to the rotation (-15 to 15)
        targetZRotation = Mathf.Lerp(-15f, 15f, karmaPoints / 100f);

        // Apply rotation to arm
        Quaternion armRotation = Quaternion.Euler(0f, 0f, targetZRotation);
        arm.localRotation = armRotation;

        // Vertical offset based on rotation
        float normalizedRotation = targetZRotation / 15f; // -1 to 1
        float leftBowlY = defaultBowlY + (-normalizedRotation * bowlYOffset);
        float rightBowlY = defaultBowlY + (normalizedRotation * bowlYOffset);

        // Apply bowl positions on the Y axis
        Vector3 leftPos = leftBowl.localPosition;
        leftBowl.localPosition = new Vector3(leftPos.x, leftBowlY, leftPos.z);

        Vector3 rightPos = rightBowl.localPosition;
        rightBowl.localPosition = new Vector3(rightPos.x, rightBowlY, rightPos.z);
    }
}
