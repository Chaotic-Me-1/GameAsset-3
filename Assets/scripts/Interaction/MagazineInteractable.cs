using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// script by OLAF - This is just a version that has been slightly altered by Ragnar for use in the Magazine
public class MagazineInteractable : MonoBehaviour, IInteractable
{
    public MagazineUI magazineUI;

    [Header("Glow Target")]
    public Renderer glowRenderer;

    private MaterialPropertyBlock propBlock;

    [Header("Glow Settings (HDRP Nits)")]
    public Color emissionColor = Color.white;
    public float emissionNitsIntensity = 2000f; 

    [Header("Audio")]
    public EventReference openMagazineEvent;

    void Start()
    {
        propBlock = new MaterialPropertyBlock();

        if (glowRenderer != null)
            DisableGlow(); 
    }

    [Header("Audio")]
    public EventReference openMagazineSound;


    public void OnTouchStart() => EnableGlow();
    public void OnTouchEnd() => DisableGlow();

    public void OnInteract()
    {
        magazineUI.ShowMagazine();

        if (!openMagazineSound.IsNull)
            RuntimeManager.PlayOneShot(openMagazineSound);

        Debug.Log("Opened Magazine: " + gameObject.name);
    }

    void EnableGlow()
    {
        if (glowRenderer == null) return;

        glowRenderer.GetPropertyBlock(propBlock);

        Color hdrColor = emissionColor.linear * emissionNitsIntensity;
        propBlock.SetColor("_EmissiveColor", hdrColor);
        propBlock.SetFloat("_EmissiveIntensity", emissionNitsIntensity);

        glowRenderer.SetPropertyBlock(propBlock);
        Debug.Log($"[Glow ON] HDRColor: {hdrColor}, Intensity: {emissionNitsIntensity}");
    }

    void DisableGlow()
    {
        if (glowRenderer == null) return;

        glowRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissiveColor", Color.black);
        propBlock.SetFloat("_EmissiveIntensity", 0f);

        glowRenderer.SetPropertyBlock(propBlock);
        Debug.Log("[Glow OFF]");
    }

    private void PlayOpenSound()
    {
        if (!openMagazineEvent.IsNull)
        {
            EventInstance instance = RuntimeManager.CreateInstance(openMagazineEvent);
            RuntimeManager.AttachInstanceToGameObject(instance, transform, GetComponent<Rigidbody>());
            instance.start();
            instance.release();
        }
    }
}