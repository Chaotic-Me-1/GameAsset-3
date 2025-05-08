using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Script by OlafRT
// Basically a tutorial that detects player input to advance through camera control, leaning, arm control, depth movement, and interaction steps. 
// Uses fading UI text and saves progress with PlayerPrefs so that you never have to see it again after having learned how to play.

public class MovementTutorial : MonoBehaviour
{
    public Text tutorialText;
    private float accumulatedMouseMovement = 0f;
    private float mouseMovementThreshold = 50f;

    private enum TutorialStep
    {
        Camera,
        Lean,
        Arm1,
        Arm2,
        Arm3,
        Done
    }

    private TutorialStep currentStep = TutorialStep.Camera;
    private float fadeDuration = 2f;
    private float displayTime = 8f;
    private bool isFading = false;
    private bool skipTutorial = false;

    private bool hasInteracted = false;

    void Start()
    {
    if (PlayerPrefs.GetInt("HasSeenTutorial", 0) == 1)
    {
        skipTutorial = true;
        tutorialText.gameObject.SetActive(false);
        currentStep = TutorialStep.Done;
        return;
    }

        ShowText("Use your mouse to look around");
    }

    void Update()
    {
        if (skipTutorial || isFading || currentStep == TutorialStep.Done) return;

        switch (currentStep)
        {
            case TutorialStep.Camera:
                float mouseDelta = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
                accumulatedMouseMovement += mouseDelta;

                if (accumulatedMouseMovement >= mouseMovementThreshold)
                {
                    StartCoroutine(AdvanceTutorial("Press W or S to lean forward/backwards in your seat.", TutorialStep.Lean));
                }
                break;

            case TutorialStep.Lean:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                {
                    StartCoroutine(AdvanceTutorial("Hold RIGHT CLICK to control your arm. Move it around by looking while holding.", TutorialStep.Arm1));
                }
                break;

            case TutorialStep.Arm1:
                if (Input.GetMouseButton(1) && (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f))
                {
                    StartCoroutine(AdvanceTutorial("SCROLL your mouse wheel while holding RIGHT CLICK to move your hand forward or backward.", TutorialStep.Arm2));
                }
                break;

            case TutorialStep.Arm2:
                if (Input.GetMouseButton(1) && Input.mouseScrollDelta.y != 0)
                {
                    StartCoroutine(AdvanceTutorial("Move your hand to something you want to interact with and press LEFT CLICK.\n(Objects glow blue when you're able to interact.)", TutorialStep.Arm3));
                }
                break;

            case TutorialStep.Arm3:
                if (hasInteracted)
                {
                    StartCoroutine(AdvanceTutorial("", TutorialStep.Done, 0f));
                }
                break;
        }
    }

    void ShowText(string message)
    {
        tutorialText.text = message;
        tutorialText.gameObject.SetActive(true);
        SetAlpha(1f);
    }

    IEnumerator AdvanceTutorial(string nextMessage, TutorialStep nextStep, float delayBeforeFade = -1f)
    {
        isFading = true;

        // Fade out message
        yield return FadeText(1f, 0f);
        currentStep = nextStep;

        if (nextStep == TutorialStep.Done)
        {
            PlayerPrefs.SetInt("HasSeenTutorial", 1);
            PlayerPrefs.Save();
            tutorialText.gameObject.SetActive(false);
        }
        else
        {
            ShowText(nextMessage);
        }

        isFading = false;
    }

    IEnumerator FadeText(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(to);
    }

    void SetAlpha(float alpha)
    {
        if (tutorialText != null)
        {
            Color color = tutorialText.color;
            color.a = alpha;
            tutorialText.color = color;
        }
    }

    // Call this from interact system when the player interacts with stuffs
    public void MarkAsInteracted()
    {
        hasInteracted = true;
    }
}
