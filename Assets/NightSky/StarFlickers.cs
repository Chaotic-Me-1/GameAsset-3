using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFlickers : MonoBehaviour
{
    public Material material; // Assign the emissive material in the Inspector
    public float minIntensity = 6f;  // Minimum emissive intensity
    public float maxIntensity = 10f; // Maximum emissive intensity
    public float flickerSpeed = 0.1f; // Speed of flickering effect (higher = slower)

    private float currentTime = 0f;

    void Update()
    {
        if (material != null)
        {
            // Calculate the flickering effect using sine wave for smooth variation
            currentTime += Time.deltaTime * flickerSpeed;
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.Sin(currentTime) * 0.5f + 0.5f);

            // Apply the new emissive intensity to the material
            material.SetFloat("_EmissiveIntensity", intensity);
        }
    }
}
