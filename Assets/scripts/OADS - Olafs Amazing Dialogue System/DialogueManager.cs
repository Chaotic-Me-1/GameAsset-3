using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour // script for managing all the dialogue within the game
{
    public static DialogueManager instance;
    public static bool IsDialogueActive { get; private set; }
    public GameObject dialoguePanel;
    public TextMeshProUGUI promptText;
    public Button[] optionButtons;
    public FirstPersonCamera playerCamera;
    public TextMeshProUGUI reactionText;
    private DialogueData currentDialogue;
    private BabyCry currentBabyCry;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData data, BabyCry baby = null)
    {
        IsDialogueActive = true;
        currentDialogue = data;
        currentBabyCry = baby;
        dialoguePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (playerCamera != null) playerCamera.cameraActive = false;

        // Clear previously selected UI
        EventSystem.current.SetSelectedGameObject(null);

        promptText.text = ParseTokens(data.promptText);

        // Play prompt voice line
        if (!currentDialogue.promptVoiceLine.IsNull)
        {
            EventInstance promptVoice = RuntimeManager.CreateInstance(currentDialogue.promptVoiceLine);
            RuntimeManager.AttachInstanceToGameObject(promptVoice, transform);
            promptVoice.start();
            promptVoice.release();
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < data.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                int index = i;
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = data.options[i].optionText;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectOption(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void SelectOption(int optionIndex)
    {
        DialogueOption choice = currentDialogue.options[optionIndex];
        KarmaManager.instance.AddKarma(choice.karmaImpact);

        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
        }

        promptText.gameObject.SetActive(false);
        reactionText.text = ParseTokens(choice.reactionText);
        reactionText.gameObject.SetActive(true);

        // player voice line
        if (!choice.playerVoiceLine.IsNull)
        {
            EventInstance playerLine = RuntimeManager.CreateInstance(choice.playerVoiceLine);
            RuntimeManager.AttachInstanceToGameObject(playerLine, transform);
            playerLine.start();
            playerLine.release();
        }

        // NPC reaction line
        StartCoroutine(PlayReactionVoiceLine(choice.npcReactionVoiceLine, 0.5f));

        // Enable objects by ID
        if (choice.objectIDsToEnable != null)
        {
            foreach (string id in choice.objectIDsToEnable)
            {
                GameObject obj = SceneObjectLinker.instance.GetObjectByID(id);
                if (obj != null)
                    obj.SetActive(true);
                else
                    Debug.LogWarning($"Object ID '{id}' not found in SceneObjectLinker.");
            }
        }

        // Branch or close
        if (choice.followUpDialogue != null)
        {
            StartCoroutine(ContinueDialogueAfterDelay(choice.followUpDialogue, 3f));
        }
        else
        {
            StartCoroutine(CloseDialogueAfterDelay(3f));
        }

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
        promptText.gameObject.SetActive(true);

        // Reactivate buttons for next dialogue
        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(true);
        }

        IsDialogueActive = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera != null) playerCamera.cameraActive = true;
    }

    private IEnumerator ContinueDialogueAfterDelay(DialogueData nextDialogue, float delay)
    {
        yield return new WaitForSeconds(delay);

        reactionText.gameObject.SetActive(false);
        promptText.gameObject.SetActive(true);

        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(true);
        }

        StartDialogue(nextDialogue); // Start follow-up dialogue
    }

    string ParseTokens(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return raw;

        // Name
        string playerName = PlayerNameManager.instance != null
                        ? PlayerNameManager.instance.GetPlayerName()
                        : "Passenger";
        raw = raw.Replace("{NAME}", playerName);

        // Date
        string today = System.DateTime.Now.ToString("dd/MM/yyyy");
        raw = raw.Replace("{DATE}", today);

        return raw;
    }
}
