using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

// Script by OlafRT
// Controls the players level of intoxication and visual effects to go along with it. 
// It uses post-processing ((HDRP) blur, vignette, distortion) and decays the drunkenness over time. 
// Other scripts can increase drunkenness, and effects dynamically scale with it

public class DrunknessManager : MonoBehaviour
{
    public static DrunknessManager instance;

    [Range(0, 100)] public float currentDrunkness = 0f; // How drunk we are
    public float decayRate = 5f; // How fast you sober up

    public Volume postProcessVolume;

    public FirstPersonBodyLook bodyLook;

    // Post-processing effects
    private DepthOfField blur;
    private Vignette vignette;
    private LensDistortion distortion;

    private float blurFade = 0f;

    void Awake()
    {
        instance = this;

        if (postProcessVolume != null)
        {
            var profile = postProcessVolume.profile;

            profile.TryGet(out blur);
            profile.TryGet(out vignette);
            profile.TryGet(out distortion);

            if (blur != null)
            {
                blur.focusMode.overrideState = true;
                blur.focusMode.value = DepthOfFieldMode.Manual;
                blur.active = true;
            }

            if (vignette != null) vignette.active = true;
            if (distortion != null) distortion.active = true;
        }
    }

    void Update()
    {
        currentDrunkness = Mathf.MoveTowards(currentDrunkness, 0f, decayRate * Time.deltaTime);
        float drunkPercent = currentDrunkness / 100f;

        // Blur only activates when your drunkness is high enough!
        if (blur != null)
        {
            bool shouldBlur = drunkPercent > 0.01f;
            blur.active = shouldBlur;

            if (shouldBlur)
            {
                float blurStrength = Mathf.Lerp(0f, 1f, drunkPercent);

                blur.nearFocusStart.value = Mathf.Lerp(0.5f, 0.3f, blurStrength);
                blur.nearFocusEnd.value   = Mathf.Lerp(1.0f, 0.5f, blurStrength);
                blur.farFocusStart.value  = Mathf.Lerp(3.0f, 1.5f, blurStrength);
                blur.farFocusEnd.value    = Mathf.Lerp(5.0f, 2.5f, blurStrength);
            }
        }

        // Vignette minimum to maximum
        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(0.32f, 0.42f, drunkPercent);

        // Lens Distortion minimum to maximum
        if (distortion != null)
            distortion.intensity.value = Mathf.Lerp(0f, -0.65f, drunkPercent);
    }

    public void ApplyDrunkness(float amount)
    {
        currentDrunkness = Mathf.Clamp(currentDrunkness + amount, 0f, 100f);
    }

    public void TriggerBlur(float intensity)
    {
        currentDrunkness = Mathf.Clamp(currentDrunkness + intensity, 0f, 100f);
    }
}