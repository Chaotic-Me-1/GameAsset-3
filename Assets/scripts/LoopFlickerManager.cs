using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LoopFlickerManager : MonoBehaviour
{
    [System.Serializable]
    public class FlickerGroup
    {
        public List<Light> lights = new List<Light>();
        public List<MeshRenderer> emissiveObjects = new List<MeshRenderer>();
        [Range(0f, 10f)] public float flickerEVRange = 1f;
    }

    [Header("Loop Flicker Configuration")]
    public List<FlickerGroup> flickerByLoop = new List<FlickerGroup>();
    public float baseEV100 = 10f;
    public float flickerSpeed = 2f;

    [Header("Karma Tinting")]
    public bool applyKarmaColorTint = true;
    public Gradient karmaColorGradient;

    private float currentFlicker = 0f;
    private Color currentTint = Color.white;
    private int currentLoop = 0;

    void Start()
    {
        if (LoopCycleManager.instance != null)
            currentLoop = LoopCycleManager.instance.loopCount;

        if (KarmaManager.instance != null && applyKarmaColorTint)
        {
            float karmaNormalized = KarmaManager.instance.karmaPoints / 100f;
            currentTint = karmaColorGradient.Evaluate(karmaNormalized);
        }
    }

    void Update()
    {
        currentFlicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

        for (int i = 0; i < flickerByLoop.Count; i++)
        {
            if (i >= currentLoop) continue;

            FlickerGroup group = flickerByLoop[i];

            // Threshold-based flicker: below threshold = OFF
            float flickerThreshold = 0.3f; // adjust for how frequent the "off" is
            bool flickerOn = currentFlicker > flickerThreshold;

            float ev100Value = flickerOn ? baseEV100 : 0f;
            float linearIntensity = flickerOn ? Mathf.Pow(2f, ev100Value) : 0f;

            foreach (Light light in group.lights)
            {
                if (light != null)
                {
                    light.intensity = linearIntensity;
                    if (applyKarmaColorTint && flickerOn)
                        light.color = currentTint;
                }
            }

            foreach (MeshRenderer rend in group.emissiveObjects)
            {
                if (rend != null)
                {
                    MaterialPropertyBlock props = new MaterialPropertyBlock();
                    float emissiveStrength = flickerOn ? Mathf.Pow(2f, ev100Value - 5f) : 0f;

                    Color emissionColor = applyKarmaColorTint ? currentTint : Color.white;
                    props.SetColor("_EmissiveColor", emissionColor * emissiveStrength);
                    rend.SetPropertyBlock(props);
                }
            }
        }
    }
}