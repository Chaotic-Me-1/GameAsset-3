using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class KarmaEnding : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI endText;
    public Image          endingImage;

    [Header("Sprites")]
    public Sprite hellSprite;
    public Sprite heavenSprite;
    public Sprite limboSprite;

    [Header("Music")]
    public AudioSource audioSource;
    public AudioClip   hellClip;
    public AudioClip   heavenClip;
    public AudioClip   limboClip;

    [Header("Loop-Reset (Limbo)")]
    public string inFlightScene = "InFlight";

    void Start()
    {
        int karma = KarmaManager.instance ? KarmaManager.instance.karmaPoints : 50;

        if (karma <= 35)                                  // ─── HELL ───
        {
            if (endText)     endText.text = "The end: Hell";
            if (endingImage) endingImage.sprite = hellSprite;
            PlayClip(hellClip);
        }
        else if (karma >= 65)                             // ─── HEAVEN ───
        {
            if (endText)     endText.text = "The end: Heaven";
            if (endingImage) endingImage.sprite = heavenSprite;
            PlayClip(heavenClip);
        }
        else                                              // ─── LIMBO ───
        {
            if (endText)     endText.text = "The End? Limbo";
            if (endingImage) endingImage.sprite = limboSprite;
            PlayClip(limboClip);

            if (LoopCycleManager.instance)
                LoopCycleManager.instance.loopCount = 0;  // reset loops

            SceneManager.LoadScene(inFlightScene);        // restart game
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
