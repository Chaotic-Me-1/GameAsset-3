using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI promptText;
    public Button[] optionButtons;
    public FirstPersonCamera playerCamera;
    public TextMeshProUGUI reactionText; // Assign in Inspector

    private DialogueData currentDialogue;
    private BabyCry currentBabyCry; // To notify after choice

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    // Start dialogue with optional BabyCry reference
    public void StartDialogue(DialogueData data, BabyCry baby = null)
    {
        currentDialogue = data;
        currentBabyCry = baby;
        dialoguePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerCamera != null) playerCamera.cameraActive = false;

        promptText.text = data.promptText;
        if (!currentDialogue.promptVoiceLine.IsNull)
        {
            EventInstance promptVoice = RuntimeManager.CreateInstance(currentDialogue.promptVoiceLine);
            RuntimeManager.AttachInstanceToGameObject(promptVoice, transform);
            promptVoice.start();
            promptVoice.release();
        }
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = data.options[i].optionText;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectOption(index));
        }
    }

    public void SelectOption(int optionIndex)
    {
        DialogueOption choice = currentDialogue.options[optionIndex];
        KarmaManager.instance.AddKarma(choice.karmaImpact);

        // Hide buttons and prompt
        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
        }

        promptText.gameObject.SetActive(false); // 👈 Hide prompt text here

        // Show reaction text
        reactionText.text = choice.reactionText;
        reactionText.gameObject.SetActive(true);

        // Play player’s selected voice line
        if (!choice.playerVoiceLine.IsNull)
        {
            EventInstance playerLine = RuntimeManager.CreateInstance(choice.playerVoiceLine);
            RuntimeManager.AttachInstanceToGameObject(playerLine, transform);
            playerLine.start();
            playerLine.release();
        }

        // Play NPC reaction voice line (delayed slightly to let player finish)
        StartCoroutine(PlayReactionVoiceLine(choice.npcReactionVoiceLine, 0.5f));

        // Start coroutine to close dialogue after a short delay
        StartCoroutine(CloseDialogueAfterDelay(3f));

        // Notify external systems
        if (currentBabyCry != null)
        {
            currentBabyCry.OnPlayerMadeChoice(optionIndex);
            currentBabyCry = null;
        }
    }

    private IEnumerator PlayReactionVoiceLine(EventReference voiceLine, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!voiceLine.IsNull)
        {
            EventInstance reaction = RuntimeManager.CreateInstance(voiceLine);
            RuntimeManager.AttachInstanceToGameObject(reaction, transform);
            reaction.start();
            reaction.release();
        }
    }
    
    private IEnumerator CloseDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        dialoguePanel.SetActive(false);
        reactionText.gameObject.SetActive(false);
        promptText.gameObject.SetActive(true); // 👈 Re-enable prompt for next use

        // Reactivate all buttons for next dialogue
        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(true);
        }

        // Resume game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera != null) playerCamera.cameraActive = true;
    }


}
