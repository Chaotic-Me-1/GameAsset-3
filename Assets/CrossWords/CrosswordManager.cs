using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.Rendering.DebugUI.Table;

//Script by Ragnar
//This is the manager for the Crossword. Responsible for the other crossword script and for populating the crossword with cells
public class CrosswordManager : MonoBehaviour
{
    public static CrosswordManager Instance;

    public GameObject cellPrefab;
    public Transform gridParent;
    public TextMeshProUGUI clueText;

    private int gridRows;
    private int gridCols;
    private CellData[,] cells;

    private Dictionary<(int, int), List<CrosswordEntry>> cellToWordsMap = new();
    private Dictionary<CrosswordEntry, List<(int, int)>> wordPaths = new();
    private CrosswordEntry currentWord;

    void Start()
    {
        Instance = this;
        gridRows = CrosswordData.GridRows;
        gridCols = CrosswordData.GridCols;

        GenerateGrid();
        PopulateCrossword();

        
        currentWord = CrosswordData.Entries.Find(e => e.ClueNumber == 1);
        if (currentWord != null)
        {
            var (r, c) = (currentWord.Row, currentWord.Col);
            HighlightClue(r, c);
            cells[r, c].inputField.Select();
        }
    }

    void GenerateGrid()
    {
        cells = new CellData[gridRows, gridCols];

        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                GameObject cellObj = Instantiate(cellPrefab, gridParent);
                CellData data = cellObj.GetComponent<CellData>();

                data.row = row;
                data.col = col;
                data.inputField = cellObj.GetComponent<TMP_InputField>();
                data.correctLetter = '\0';

                data.inputField.text = "";
                data.inputField.interactable = false;
                data.inputField.image.color = Color.black;

                var placeholder = data.inputField.placeholder?.GetComponent<TextMeshProUGUI>();
                if (placeholder != null)
                {
                    placeholder.text = "";
                    placeholder.color = Color.clear;
                }

                cells[row, col] = data;
            }
        }
    }

    void PopulateCrossword()
    {
        foreach (var entry in CrosswordData.Entries)
        {
            string word = entry.Word.ToUpper();
            List<(int, int)> path = new();

            for (int i = 0; i < word.Length; i++)
            {
                int row = entry.Row;
                int col = entry.Col;

                if (entry.Direction == Direction.Across) col += i;
                else if (entry.Direction == Direction.Down) row += i;

                CellData cell = cells[row, col];

                cell.correctLetter = word[i];
                cell.inputField.interactable = true;
                cell.inputField.characterLimit = 1;
                cell.inputField.image.color = Color.white;

                path.Add((row, col));

                if (!cellToWordsMap.ContainsKey((row, col)))
                    cellToWordsMap[(row, col)] = new List<CrosswordEntry>();

                cellToWordsMap[(row, col)].Add(entry);

                if (i == 0)
                {
                    AddClueNumberToCell(cell, entry.ClueNumber);
                }
            }

            wordPaths[entry] = path;
        }
    }

    void AddClueNumberToCell(CellData cell, int clueNumber)
    {
        GameObject numberObj = new GameObject("ClueNumber");
        numberObj.transform.SetParent(cell.transform, false);
        numberObj.transform.SetAsFirstSibling();

        TextMeshProUGUI text = numberObj.AddComponent<TextMeshProUGUI>();
        text.text = clueNumber.ToString();
        text.fontSize = 20; // THIS IS WHERE I CHANGE FONT SIZE!!!
        text.enableAutoSizing = true;
        text.alignment = TextAlignmentOptions.TopLeft;
        text.color = Color.black; // CHANGE FONT COLOR HERE

        RectTransform rt = numberObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(5, -5);
        rt.sizeDelta = new Vector2(30, 30);
    }

    public void HighlightClue(int row, int col)
    {
        if (cellToWordsMap.TryGetValue((row, col), out List<CrosswordEntry> entries))
        {
            CrosswordEntry entry = entries[0];
            clueText.text = $"{entry.ClueNumber}. {entry.Clue}";
        }
    }

    public void MoveToNextCellInWord(int currentRow, int currentCol)
    {
        if (currentWord == null) return;

        int nextRow = currentRow;
        int nextCol = currentCol;

        if (currentWord.Direction == Direction.Across)
            nextCol++;
        else if (currentWord.Direction == Direction.Down)
            nextRow++;

        foreach (var (r, c) in wordPaths[currentWord])
        {
            if (r == nextRow && c == nextCol)
            {
                cells[nextRow, nextCol].inputField.Select();
                return;
            }
        }
    }

    public void MoveFocus(int targetRow, int targetCol)
    {
        if (targetRow < 0 || targetRow >= gridRows || targetCol < 0 || targetCol >= gridCols)
            return;

        var next = cells[targetRow, targetCol];
        if (next != null && next.inputField.interactable)
        {
            next.inputField.Select();
        }
    }

    public void SelectWordFromCell(int row, int col)
    {
        if (cellToWordsMap.TryGetValue((row, col), out List<CrosswordEntry> entries))
        {
            foreach (var entry in entries)
            {
                if (currentWord != null && entry.Direction == currentWord.Direction)
                {
                    currentWord = entry;
                    HighlightClue(row, col);
                    return;
                }
            }

            currentWord = entries[0];
            HighlightClue(row, col);
        }
    }

    public void CheckAnswers()
    {
        foreach (var cell in cells)
        {
            if (!cell.inputField.interactable) continue;

            string input = cell.inputField.text.ToUpper();
            if (input == cell.correctLetter.ToString())
                cell.inputField.image.color = Color.green;
            else
                cell.inputField.image.color = Color.red;
        }
    }
}
