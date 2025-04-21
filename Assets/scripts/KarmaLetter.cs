using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class KarmaLetterInteractable : MonoBehaviour, IInteractable
{
    /*───────────────────────────  INSPECTOR  ───────────────────────────*/

    [Header("UI")]
    public GameObject        letterPanel;
    public TextMeshProUGUI   letterText;
    [Space]
    public Button            closeButton;
    public Image             fadeImage;

    [Header("Glow (HDRP)")]
    public Color  emissionColor         = Color.white;
    public float  emissionNitsIntensity = 2000f;

    [Header("Audio")]
    public EventReference openLetterEvent;

    [Header("End‑Credits")]
    public string creditsSceneName = "Credits";
    public float  fadeDuration     = 3f;

    [Header("Loop Restart (Limbo)")]
    public string firstLoopScene = "InFlight";   // scene that represents loop‑0

    /*───────────────────────────  PRIVATE  ─────────────────────────────*/

    Renderer               rend;
    MaterialPropertyBlock  propBlock;

    string destination;                                 // "Heaven" | "Hell" | "Limbo"

    /*───────────────────────────  MONO  ───────────────────────────────*/

    void Start()
    {
        rend      = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        DisableGlow();

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseLetter);

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    /*───────────────────────────  INTERACT  ───────────────────────────*/

    public void OnTouchStart() => EnableGlow();
    public void OnTouchEnd()   => DisableGlow();

    public void OnInteract()
    {
        if (letterPanel == null || letterText == null) return;

        letterText.text = BuildLetterBody();

        letterPanel.SetActive(true);
        Cursor.visible   = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale   = 0f;

        if (!openLetterEvent.IsNull)
            RuntimeManager.PlayOneShot(openLetterEvent);
    }

    /*───────────────────────────  CLOSE  ──────────────────────────────*/

    public void CloseLetter()
    {
        letterPanel.SetActive(false);

        // decide what happens after the fade
        if (destination == "Limbo")
            StartCoroutine(RestartAtFirstLoop());
        else
            StartCoroutine(EndCreditsSequence());
    }

    /*───────────────────────────  COROUTINES  ─────────────────────────*/

    IEnumerator EndCreditsSequence()
    {
        yield return FadeOut();

        SceneManager.LoadScene(creditsSceneName);
    }

    IEnumerator RestartAtFirstLoop()
    {
        yield return FadeOut();

        // reset loop counter if we use LoopCycleManager
        if (LoopCycleManager.instance != null)
            LoopCycleManager.instance.loopCount = 0;

        SceneManager.LoadScene(firstLoopScene);
    }

    IEnumerator FadeOut()
    {
        Time.timeScale   = 1f;
        Cursor.visible   = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                float a = Mathf.Clamp01(t / fadeDuration);
                fadeImage.color = new Color(c.r, c.g, c.b, a);
                yield return null;
            }
        }
    }

    /*───────────────────────────  LETTER  ─────────────────────────────*/

    string BuildLetterBody()
    {
        string playerName = !string.IsNullOrEmpty(PlayerNameManager.instance?.playerName)
                            ? PlayerNameManager.instance.playerName
                            : "Passenger";

        int karma = KarmaManager.instance != null ? KarmaManager.instance.karmaPoints : 50;

        destination =
            karma <= 35 ? "Hell"  :
            karma >= 65 ? "Heaven":
                          "Limbo";

        return
$@"Dear {playerName},

I must regret to inform you that you have died.  
This is the after‑life, and you are on a journey.

I'm sure this must be confusing, perhaps even frightening.  
Please understand that the actions you have taken here  
reflect upon you as a person.

Therefore, you are chosen to go to **{destination}**.

You cannot appeal this decision; it is final.  
May eternity there serve you well.";
    }

    /*───────────────────────────  GLOW  ──────────────────────────────*/

    void EnableGlow()
    {
        if (rend == null) return;
        rend.GetPropertyBlock(propBlock);
        Color hdr = emissionColor.linear * emissionNitsIntensity;
        propBlock.SetColor("_EmissiveColor", hdr);
        propBlock.SetFloat("_EmissiveIntensity", emissionNitsIntensity);
        rend.SetPropertyBlock(propBlock);
    }

    void DisableGlow()
    {
        if (rend == null) return;
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissiveColor", Color.black);
        propBlock.SetFloat("_EmissiveIntensity", 0f);
        rend.SetPropertyBlock(propBlock);
    }
}