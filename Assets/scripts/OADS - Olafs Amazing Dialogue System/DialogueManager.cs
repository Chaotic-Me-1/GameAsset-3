using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI promptText;
    public Button[] optionButtons;
    public FirstPersonCamera playerCamera;

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

        Debug.Log("NPC Reaction: " + choice.reactionText);

        dialoguePanel.SetActive(false);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera != null) playerCamera.cameraActive = true;
        // Notify the baby (or other system)
        if (currentBabyCry != null)
        {
            currentBabyCry.OnPlayerMadeChoice(optionIndex);
            currentBabyCry = null; // Reset after use
        }

    }
}
