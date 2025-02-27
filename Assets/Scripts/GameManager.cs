using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] List<TextAsset> savedPuzzleFiles;
    [SerializeField] Transform gridParent, rowClueParent, colClueParent;
    [SerializeField] GameObject rowCluePrefab, colCluePrefab, cellButtonPrefab;
    [Header("Win")]
    [SerializeField] GameObject victoryPanel;
    [SerializeField] Button victoryButton;
    int puzzleIndex = 0;
    int rows, columns;

    NonogramPuzzle puzzle;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        victoryButton.onClick.AddListener(BackToOverworld);
        victoryPanel.SetActive(false);

        SetPuzzleIndex();
        // Nonogram puzzle scene loads in and starts the timer
        TimerScript.instance.BeginTimer();
    }

    void SetPuzzleIndex()
    {
        //After set

        //Load puzzle
        LoadCurrentPuzzle();
    }

    void LoadCurrentPuzzle()
    {
        puzzle = LoadPuzzle();
        if(puzzle != null)
        {
            //Generate new puzzle
            GeneratePuzzle();
        }
    }

    NonogramPuzzle LoadPuzzle()
    {
        if(LevelLoader.puzzleToLoad != null)
        {
            return LevelLoader.puzzleToLoad;
        }
        TextAsset selectedPuzzle = savedPuzzleFiles[puzzleIndex];
        string json = selectedPuzzle.text;
        NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(json);
        return loadedPuzzle;
    }

    void GeneratePuzzle()
    {
        rows = puzzle.Rows;
        columns = puzzle.Cols;

        gridParent.GetComponent<GridLayoutGroup>().constraintCount = columns;
        //Clear existing content
        ClearBoard();
        //Create row clues
        GenerateRowClues();
        //Create column clues
        GenerateColumnClues();
        //Create grid
        GenerateGrid();
    }

    void ClearBoard()
    {
        foreach(Transform child in gridParent)
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
    }

    void GenerateRowClues()
    {
        for(int r = 0; r < rows; r++)
        {
            GameObject rowClue = Instantiate(rowCluePrefab, rowClueParent);
            rowClue.GetComponentInChildren<TMP_Text>().text = string.Join("", puzzle.RowClues[r].Clues);
        }
    }

    void GenerateColumnClues()
    {
        for (int c = 0; c < columns; c++)
        {
            GameObject colClue = Instantiate(colCluePrefab, colClueParent);
            colClue.GetComponentInChildren<TMP_Text>().text = string.Join("\n", puzzle.ColClues[c].Clues);
        }
    }

    void GenerateGrid()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject cell = Instantiate(cellButtonPrefab, gridParent);
                ButtonScript cellButton = cell.GetComponent<ButtonScript>();
                cellButton.row = r;
                cellButton.col = c;
                cellButton.puzzle = puzzle;
            }
        }
    }

    public void CheckWinCondition()
    {
        for (int r = 0; r < puzzle.Rows; r++)
        {
            for (int c = 0; c < puzzle.Cols; c++)
            {
                if (puzzle.SolutionData[r,c] == 1 && puzzle.GridData[r,c] != 1 ||
                    puzzle.SolutionData[r,c] == 0 && puzzle.GridData[r,c] == 1)
                {
                    //Puzzle not solved;
                    return;
                }
            }
        }
        //Game is won
        //Show win screen
        victoryPanel.SetActive(true);
        TimerScript.instance.PauseTimer();
    }

    void BackToOverworld()
    {
        SceneManager.LoadScene("MapScene");
    }
}
