using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HEAVEN : MonoBehaviour
{
    [Header("Karma / Loop")]
    public int karmaThreshold = 65;
    public int requiredLoop   = 3;

    [Header("Objects")]
    public GameObject objectToActivate;
    public GameObject objectToDisable;
    public Transform  planeRoot;
    public float      ascendDuration = 8;

    [Header("Sky & Clouds")]
    public Volume skyFogVolume;

    [Header("Directional Light (Sun)")]
    public Light  sunLight;
    public float  initialFlareSize = 2f;
    public float  targetFlareSize  = 50f;
    public float  flareGrowTime    = 15f;

    PhysicallyBasedSky    sky;
    VolumetricClouds      clouds;
    HDAdditionalLightData hdLight;

    void Start()
    {
        if (!ConditionsMet()) return;

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        if (objectToDisable != null)
            objectToDisable.SetActive(false);

        ApplySkyChanges();

        if (planeRoot != null)
            StartCoroutine(PitchPlaneRoutine());

        if (hdLight != null)
            StartCoroutine(AnimateFlareRoutine());
    }

    bool ConditionsMet()
    {
        int karma = KarmaManager.instance      ? KarmaManager.instance.karmaPoints   : 0;
        int loop  = LoopCycleManager.instance ? LoopCycleManager.instance.loopCount : 0;
        return karma >= karmaThreshold && loop == requiredLoop;
    }

    void ApplySkyChanges()
    {
        if (skyFogVolume != null && skyFogVolume.profile.TryGet(out sky))
        {
            Color gold = new Color(1f, 0.96f, 0.83f);

            sky.aerosolTint.overrideState =
            sky.horizonTint.overrideState =
            sky.zenithTint .overrideState = true;

            sky.aerosolTint.value = sky.horizonTint.value = sky.zenithTint.value = gold;
        }

        if (skyFogVolume != null && skyFogVolume.profile.TryGet(out clouds))
            clouds.active = true;

        if (sunLight != null)
        {
            hdLight = sunLight.GetComponent<HDAdditionalLightData>();
            if (hdLight != null)
                hdLight.flareSize = initialFlareSize;
            else
                Debug.LogWarning($"{name}: Sun light has no HDAdditionalLightData");
        }
    }

    IEnumerator PitchPlaneRoutine()
    {
        Quaternion startRot  = planeRoot.localRotation;
        Quaternion targetRot = Quaternion.Euler(+45f, startRot.eulerAngles.y, startRot.eulerAngles.z);

        float t = 0f;
        while (t < ascendDuration)
        {
            t += Time.deltaTime;
            planeRoot.localRotation = Quaternion.Slerp(startRot, targetRot, t / ascendDuration);
            yield return null;
        }
        planeRoot.localRotation = targetRot;
    }

    IEnumerator AnimateFlareRoutine()
    {
        if (hdLight == null) yield break;

        float t = 0f;
        while (t < flareGrowTime)
        {
            t += Time.deltaTime;
            hdLight.flareSize = Mathf.Lerp(initialFlareSize, targetFlareSize, t / flareGrowTime);
            yield return null;
        }
        hdLight.flareSize = targetFlareSize;
    }
}