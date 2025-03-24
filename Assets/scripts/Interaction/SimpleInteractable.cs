using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    private Renderer rend;
    private Material originalMat;
    public Material glowMaterial;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMat = rend.material;
    }

    public void OnTouchStart()
    {
        rend.material = glowMaterial;
    }

    public void OnTouchEnd()
    {
        rend.material = originalMat;
    }

    public void OnInteract()
    {
        Debug.Log("Interacted with: " + gameObject.name);
        // Do something here!
    }
}