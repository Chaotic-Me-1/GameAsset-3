using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UI;

public class CellData : MonoBehaviour
{
    public int row;
    public int col;
    public char correctLetter;
    public TMP_InputField inputField;

    void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(OnValueChanged);
        inputField.onSelect.AddListener(OnSelected);
    }

    void OnSelected(string _)
    {
        CrosswordManager.Instance.SelectWordFromCell(row, col);
    }


    public void OnValueChanged(string value)
    {
        if (value.Length == 1)
        {
            CrosswordManager.Instance.HighlightClue(row, col);
            CrosswordManager.Instance.MoveToNextCellInWord(row, col);
        }
    }


    void Update()
    {
        if (!inputField.isFocused) return;

        // Arrow keys
        if (Input.GetKeyDown(KeyCode.RightArrow))
            CrosswordManager.Instance.MoveFocus(row, col + 1);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CrosswordManager.Instance.MoveFocus(row, col - 1);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            CrosswordManager.Instance.MoveFocus(row + 1, col);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            CrosswordManager.Instance.MoveFocus(row - 1, col);

        // Backspace
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inputField.text = "";
            CrosswordManager.Instance.MoveFocus(row, col - 1);
        }
    }
}
