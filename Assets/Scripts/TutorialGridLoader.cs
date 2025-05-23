using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TutorialGridLoader : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private string puzzleFileName = "TutorialPuz"; // no extension
    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform rowClueParent;
    [SerializeField] private Transform colClueParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject rowCluePrefab;
    [SerializeField] private GameObject colCluePrefab;

    private NonogramPuzzle tutorialPuzzle;

    void Start()
    {
        LoadPuzzle();
        if (tutorialPuzzle != null)
        {
            GenerateTutorialGrid();
        }
    }

    void LoadPuzzle()
    {
        TextAsset puzzleJson = Resources.Load<TextAsset>("SavedPuzzles/" + puzzleFileName);
        if (puzzleJson != null)
        {
            tutorialPuzzle = JsonUtility.FromJson<NonogramPuzzle>(puzzleJson.text);
        }
        else
        {
            Debug.LogError("Tutorial puzzle not found in Resources/SavedPuzzles: " + puzzleFileName);
        }
    }


    void GenerateTutorialGrid()
    {
        ClearGrid();

        int rows = tutorialPuzzle.Rows;
        int cols = tutorialPuzzle.Cols;

        // grid layout
        GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = cols;
        }

        // row clues
        for (int r = 0; r < rows; r++)
        {
            GameObject rowClue = Instantiate(rowCluePrefab, rowClueParent);
            rowClue.GetComponent<TMP_Text>().text = string.Join(" ", tutorialPuzzle.RowClues[r].Clues);
        }

        // column clues
        for (int c = 0; c < cols; c++)
        {
            GameObject colClue = Instantiate(colCluePrefab, colClueParent);
            colClue.GetComponent<TMP_Text>().text = string.Join("\n", tutorialPuzzle.ColClues[c].Clues);
        }

        // generate 
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                cell.name = $"Cell {r},{c}";

                ButtonScript cellButton = cell.GetComponent<ButtonScript>();

               //ADDED BY LANCE
               //****************************************************************************
                cellButton.row = r;
                cellButton.col = c;
                cellButton.puzzle = tutorialPuzzle;
                //***************************************************************************

                // gray out cells to make them non-interactive in the tutorial
                Button btn = cell.GetComponent<Button>();
                if (btn) btn.interactable = false;
            }
        }
    }

    void ClearGrid()
    {
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        foreach (Transform child in rowClueParent) Destroy(child.gameObject);
        foreach (Transform child in colClueParent) Destroy(child.gameObject);
    }
}
