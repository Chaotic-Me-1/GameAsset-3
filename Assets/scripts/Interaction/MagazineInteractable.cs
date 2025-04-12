using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MagazineInteractable : MonoBehaviour, IInteractable
{
    public MagazineUI magazineUI;

    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    [Header("Glow Settings (HDRP Nits)")]
    public Color emissionColor = Color.white;
    public float emissionNitsIntensity = 2000f; // Strong glow

    [Header("Audio")]
    public EventReference openMagazineEvent;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        if (rend != null)
            DisableGlow(); // Only try glow logic if Renderer exists
    }


    public void OnTouchStart() => EnableGlow();
    public void OnTouchEnd() => DisableGlow();

    public void OnInteract()
    {
        magazineUI.ShowMagazine();
        PlayOpenSound();
        Debug.Log("Opened Magazine: " + gameObject.name);
    }

    void EnableGlow()
    {
        if (rend == null) return;

        rend.GetPropertyBlock(propBlock);

        Color hdrColor = emissionColor.linear * emissionNitsIntensity;
        propBlock.SetColor("_EmissiveColor", hdrColor);
        propBlock.SetFloat("_EmissiveIntensity", emissionNitsIntensity);

        rend.SetPropertyBlock(propBlock);
        Debug.Log($"[Glow ON] HDRColor: {hdrColor}, Intensity: {emissionNitsIntensity}");
    }


    void DisableGlow()
    {
        if (rend == null) return;

        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissiveColor", Color.black);
        propBlock.SetFloat("_EmissiveIntensity", 0f);

        rend.SetPropertyBlock(propBlock);
        Debug.Log("[Glow OFF]");
    }


    private void PlayOpenSound()
    {
        if (!openMagazineEvent.IsNull)
        {
            EventInstance instance = RuntimeManager.CreateInstance(openMagazineEvent);
            RuntimeManager.AttachInstanceToGameObject(instance, transform, GetComponent<Rigidbody>());
            instance.start();
            instance.release(); // Let FMOD handle cleanup
        }
    }
}
