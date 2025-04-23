using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HELL : MonoBehaviour
{
    [Header("Karma")]
    [Tooltip("Karma must be <= this to trigger.")]
    public int karmaThreshold = -35;
    [Tooltip("Loop")]
    public int requiredLoop = 3;

    [Header("Objects")]
    public GameObject objectToActivate;
    public Transform planeRoot;
    public float descendDuration = 8f;

    [Header("Volume")]
    public Volume skyFogVolume;
    public Volume globalVolume;

    [Header("Vignette animation")]
    [Range(0f,1f)] public float vignetteMin = 0.40f;
    [Range(0f,1f)] public float vignetteMax = 0.50f;
    public float vignettePeriod = 1f;

    PhysicallyBasedSky sky;
    VolumetricClouds   clouds;
    Vignette           vignette;
    bool               animateVignette = false;

    void Start()
    {
        if (!ConditionsMet()) return;

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        ApplySkyChanges();
        SetupVignetteAnimation();
        if (planeRoot != null)
            StartCoroutine(PitchPlaneRoutine());
    }

    bool ConditionsMet()
    {
        int karma = KarmaManager.instance   != null ? KarmaManager.instance.karmaPoints : 0;
        int loop  = LoopCycleManager.instance != null ? LoopCycleManager.instance.loopCount : 0;
        return karma <= karmaThreshold && loop == requiredLoop;
    }

    void ApplySkyChanges()
    {
        if (skyFogVolume == null) return;

        var profile = skyFogVolume.profile;

        if (profile.TryGet(out sky))
        {
            sky.aerosolTint .overrideState =
            sky.horizonTint .overrideState =
            sky.zenithTint  .overrideState = true;

            var evilRed = Color.red;
            sky.aerosolTint.value = sky.horizonTint.value = sky.zenithTint.value = evilRed;
        }

        if (profile.TryGet(out clouds))
            clouds.active = false;
    }

    void SetupVignetteAnimation()
    {
        if (globalVolume == null) return;
        if (globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
            animateVignette = true;
        }
    }

    void Update()
    {
        if (animateVignette && vignette != null)
        {
            float t = Mathf.PingPong(Time.time, vignettePeriod) / vignettePeriod;
            vignette.intensity.value = Mathf.Lerp(vignetteMin, vignetteMax, t);
        }
    }

    System.Collections.IEnumerator PitchPlaneRoutine()
    {
        Quaternion startRot = planeRoot.localRotation;
        Quaternion targetRot = Quaternion.Euler(-45f, startRot.eulerAngles.y, startRot.eulerAngles.z);

        float t = 0f;
        while (t < descendDuration)
        {
            t += Time.deltaTime;
            float k = t / descendDuration;
            planeRoot.localRotation = Quaternion.Slerp(startRot, targetRot, k);
            yield return null;
        }
        planeRoot.localRotation = targetRot;
    }
}
