using UnityEngine;

public class LeafletInteractable : MonoBehaviour, IInteractable
{
    public LeafletUI leafletUI;

    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    [Header("Glow Settings (HDRP Nits)")]
    public Color emissionColor = Color.white;
    public float emissionNitsIntensity = 2000f; // Strong glow

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
        leafletUI.ShowLeaflet();
        Debug.Log("Opened Leaflet: " + gameObject.name);
    }

    void EnableGlow()
    {
        rend.GetPropertyBlock(propBlock);

        Color hdrColor = emissionColor.linear * emissionNitsIntensity;
        propBlock.SetColor("_EmissiveColor", hdrColor);
        propBlock.SetFloat("_EmissiveIntensity", emissionNitsIntensity);

        rend.SetPropertyBlock(propBlock);
        Debug.Log($"[Glow ON] HDRColor: {hdrColor}, Intensity: {emissionNitsIntensity}");
    }

    void DisableGlow()
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissiveColor", Color.black);
        propBlock.SetFloat("_EmissiveIntensity", 0f);

        rend.SetPropertyBlock(propBlock);
        Debug.Log("[Glow OFF]");
    }
}


