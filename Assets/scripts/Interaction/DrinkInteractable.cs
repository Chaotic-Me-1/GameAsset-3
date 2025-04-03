using UnityEngine;
using FMODUnity;

public class DrinkInteractable : MonoBehaviour, IInteractable
{
    [Range(0, 100)]
    public float alcoholStrength = 0f;

    public Transform drinkHoldOffset;
    public Transform followTarget;

    [Header("References")]
    public DrunknessManager drunknessManager;
    public GameObject hintTextUI;
    public GameObject objectToDestroyOnDrink; // 🍷 like the liquid inside
    public EventReference drinkSound;         // 🎧 FMOD one-shot

    [Header("Hand Pose")]
    public Transform jointToTwist; // 👋 Assign your wrist/thumb/etc. joint here
    public Vector3 jointRotationOffset = new Vector3(0f, -30f, 0f); // example offset
    private Quaternion originalJointRotation;
    
    private bool isTwistingJoint = false;
    private bool isHeld = false;
    private bool hasBeenDrunk = false;
    private Rigidbody rb;
    private Transform originalParent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent;

        if (hintTextUI != null)
            hintTextUI.SetActive(false);

        if (drunknessManager == null)
            Debug.LogWarning("DrunknessManager not assigned!", this);
    }

    public void OnTouchStart() { }
    public void OnTouchEnd() { }

    public void OnInteract()
    {
        if (isHeld || followTarget == null)
            return;

        transform.SetParent(followTarget);

        if (drinkHoldOffset != null)
        {
            transform.position = drinkHoldOffset.position;
            transform.rotation = drinkHoldOffset.rotation;
        }
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        isHeld = true;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (hintTextUI != null && !hasBeenDrunk)
            hintTextUI.SetActive(true); // Only show hint if there's something to drink

        if (jointToTwist != null)
        {
            originalJointRotation = jointToTwist.localRotation;
            isTwistingJoint = true;
        }
    }

    void Update()
    {
        // ☕️ Press E to drink only if not already drunk
        if (isHeld && !hasBeenDrunk && Input.GetKeyDown(KeyCode.E))
        {
            // 🎧 Play drink sound
            if (!drinkSound.IsNull)
            {
                FMOD.Studio.EventInstance sfx = RuntimeManager.CreateInstance(drinkSound);
                RuntimeManager.AttachInstanceToGameObject(sfx, transform);
                sfx.start();
                sfx.release();
            }

            // 💫 Get tipsy
            if (drunknessManager != null)
            {
                drunknessManager.ApplyDrunkness(alcoholStrength);
                drunknessManager.TriggerBlur(alcoholStrength);
            }

            // 🍷 Hide liquid only
            if (objectToDestroyOnDrink != null)
            {
                objectToDestroyOnDrink.SetActive(false);
            }

            hasBeenDrunk = true;

            if (hintTextUI != null)
                hintTextUI.SetActive(false); // Hide prompt after drinking
        }

        // 🖱 Let go of the object
        if (isHeld && Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }
    
    void LateUpdate()
    {
        if (isTwistingJoint && jointToTwist != null)
        {
            jointToTwist.localRotation = originalJointRotation * Quaternion.Euler(jointRotationOffset);
        }
    }

    private void Release()
    {
        isHeld = false;
        transform.SetParent(originalParent);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (hintTextUI != null)
            hintTextUI.SetActive(false);

        if (jointToTwist != null)
        {
            isTwistingJoint = false;
            jointToTwist.localRotation = originalJointRotation;
        }
    }
}