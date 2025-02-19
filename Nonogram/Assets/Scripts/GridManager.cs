using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] int rows = 4, cols = 4;
    [SerializeField] Transform gridParent, rowClueParent, colClueParent;
    [SerializeField] GameObject cellButtonPrefab, rowCluePrefab, colCluePrefab;
    //Connection to puzzle
    NonogramPuzzle puzzle;

    string filepath = Application.dataPath + "/SavedPuzzles/";
    [SerializeField] TMP_InputField puzzleNameInput;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //new puzzle
        puzzle = new NonogramPuzzle(rows, cols);
        //generate grid
        GenerateGrid();
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = cols;
    }

    public void ResetPuzzle()
    {
        //new puzzle

        puzzleNameInput.text = "";
        puzzle = new NonogramPuzzle(rows, cols);
        //generate grid
        GenerateGrid();
        gridParent.GetComponent<GridLayoutGroup>().constraintCount = cols;
    }

    void GenerateGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in colClueParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rowClueParent)
        {
            Destroy(child.gameObject);
        }
        //Generate new cells
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject cell = Instantiate(cellButtonPrefab, gridParent);
                cell.name = $"cell {r},{c}";

                ButtonScript cellButton = cell.GetComponent<ButtonScript>();
                cellButton.row = r;
                cellButton.col = c;
                //Add corresponding puzzle
                cellButton.puzzle = puzzle;
            }
        }
        //Row clues
        for(int r = 0; r < rows; r++)
        {
            GameObject rowClue = Instantiate(rowCluePrefab, rowClueParent);
            rowClue.name = $"row clue {r}";
            rowClue.GetComponent<TMP_Text>().text = "0";
        }
        //Column clues
        for (int c = 0; c < rows; c++)
        {
            GameObject colClue = Instantiate(colCluePrefab, colClueParent);
            colClue.name = $"row clue {c}";
            colClue.GetComponent<TMP_Text>().text = "0";
        }
    }

    public void OnCellStateChanged()
    {
        //Regenerate the clues
        GenerateClues();
        //Update the clues UI
        CluesUIManager.instance.UpdateClues(puzzle);
    }

    void GenerateClues()
    {
        int rr = puzzle.SolutionData.GetLength(0);
        int cc = puzzle.SolutionData.GetLength(1);
        //Row clues
        for (int r = 0; r < rr; r++)
        {
            puzzle.RowClues[r] = new CluesWrapper {Clues = GetCluesForLine(puzzle.SolutionData,r,true)};
        }

        //Column clues
        for (int c = 0; c < cc; c++)
        {
            puzzle.ColClues[c] = new CluesWrapper {Clues = GetCluesForLine(puzzle.SolutionData,c,false)};
        }
    }

    List<int> GetCluesForLine(int[,] gridData, int index, bool isRow)
    {
        List<int> clues = new List<int>();
        int count = 0;

        int length = isRow ? gridData.GetLength(1) : gridData.GetLength(0);

        for (int i = 0; i < length; i++)
        {
            int value = isRow ? gridData[index, i] : gridData[i, index];
            if(value == 1)
            {
                ++count;
            }
            else if(count>0)
            {
                clues.Add(count);
                count = 0;
            }
        }

        //If there is a leftover
        if(count > 0)
        {
            clues.Add(count);
        }

        //If no clue found, add 0
        if(clues.Count == 0)
        {
            clues.Add(0);
        }

        return clues;
    }

    //-----Saving and Loading--------------------------------------------
    //Called from a button
    public void SavePuzzle()
    {
        if (string.IsNullOrEmpty(puzzleNameInput.text))
        {
            Debug.LogError("Must enter a valid puzzle name before saving...");
            return;
        }
        puzzle.SolutionData = puzzle.SolutionData;
        string json = JsonUtility.ToJson(puzzle, true);
        string fullpath = filepath + puzzleNameInput.text + ".json";
        System.IO.File.WriteAllText(fullpath, json);
        Debug.Log($"Puzzle Saved to {fullpath}");
    }

}
