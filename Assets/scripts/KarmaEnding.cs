using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class KarmaEnding : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI endText;
    public Image endingImage;

    [Header("Sprites")]
    public Sprite hellSprite;
    public Sprite heavenSprite;
    public Sprite limboSprite;

    [Header("Loop‑Reset (Limbo)")]
    public string inFlightScene = "InFlight";

    void Start()
    {
        int karma = KarmaManager.instance ? KarmaManager.instance.karmaPoints : 50;

        if (karma <= 35)
        {
            if (endText)   endText.text   = "The end: Hell";
            if (endingImage && hellSprite)   endingImage.sprite = hellSprite;
        }
        else if (karma >= 65)
        {
            if (endText)   endText.text   = "The end: Heaven";
            if (endingImage && heavenSprite) endingImage.sprite = heavenSprite;
        }
        else // limbo  (36‑64)
        {
            if (endText)   endText.text   = "The End? Limbo";
            if (endingImage && limboSprite)  endingImage.sprite = limboSprite;

            // restart game at first loop
            if (LoopCycleManager.instance) LoopCycleManager.instance.loopCount = 0;
            SceneManager.LoadScene(inFlightScene);
        }
    }
}