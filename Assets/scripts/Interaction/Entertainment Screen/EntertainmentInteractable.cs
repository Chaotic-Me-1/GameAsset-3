using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script by OlafRT
// The interactable part of the entertainment system, this is the part that lets us open it
// by touching it and clicking on it with our hand. Like the other interactable stuff, this glows when touched and then
// opens UI when clicked on.

public class EntertainmentInteractable : MonoBehaviour, IInteractable
{
    public EntertainmentUIManager uiManager;

    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    [Header("Glow")]
    public Color emissionColor = Color.white;
    public float emissionNitsIntensity = 2000f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        DisableGlow();
    }

    public void OnTouchStart() => EnableGlow();
    public void OnTouchEnd() => DisableGlow();

    public void OnInteract()
    {
        if (uiManager != null)
            uiManager.ShowEntertainmentUI();
        Debug.Log("Opened Entertainment: " + gameObject.name);
    }

    void EnableGlow()
    {
        rend.GetPropertyBlock(propBlock);
        Color hdrColor = emissionColor.linear * emissionNitsIntensity;
        propBlock.SetColor("_EmissiveColor", hdrColor);
        propBlock.SetFloat("_EmissiveIntensity", emissionNitsIntensity);
        rend.SetPropertyBlock(propBlock);
    }

    void DisableGlow()
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissiveColor", Color.black);
        propBlock.SetFloat("_EmissiveIntensity", 0f);
        rend.SetPropertyBlock(propBlock);
    }
}