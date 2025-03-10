using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] List<TextAsset> savedPuzzleFiles;
    [SerializeField] List<Sprite> victorySprites;
    [SerializeField] Transform gridParent, rowClueParent, colClueParent;
    [SerializeField] GameObject rowCluePrefab, colCluePrefab, cellButtonPrefab;

    [Header("Win")]
    [SerializeField] GameObject victoryPanel;
    [SerializeField] Button victoryButton;
    Sprite victoryScreenSprite;
    public SolutionPanelScript solutionScreen;

    int puzzleIndex = 0;
    int rows, columns;

    AudioManager sounds;
    NonogramPuzzle puzzle;

    string saveProgressPath = Application.dataPath + "/ProgressPuzzles/";

    Dictionary<int, string> matchPuzzle = new Dictionary<int, string>()
    {
        {0, "PsycheLogo"},
        {1, "Spacecraft"},
        {2, "Asteroid"},
        {3, "Instruments"}
    };

    

    private void Awake()
    {
        Instance = this;
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        victoryButton.onClick.AddListener(BackToOverworld);
        victoryPanel.SetActive(false);

        SetPuzzleIndex();
    }

    void SetPuzzleIndex()
    {
        // After set

        // Load puzzle
        LoadCurrentPuzzle();
    }

    void LoadCurrentPuzzle()
    {
        puzzle = LoadPuzzle();
        if (puzzle != null)
        {
            // Generate new puzzle
            GeneratePuzzle();
        }
    }

    NonogramPuzzle LoadPuzzle()
    {
        
        if (LevelLoader.puzzleToLoad != null)
        {
            // Assign an index number to each puzzle by name
            puzzleIndex = matchPuzzle.FirstOrDefault(x => x.Value == LevelLoader.puzzleName).Key;

            string savedFilePath = saveProgressPath + LevelLoader.puzzleName + ".json";
            if (System.IO.File.Exists(savedFilePath))
            {
                string json = System.IO.File.ReadAllText(savedFilePath);
                NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(json);
                return loadedPuzzle;
            }

            return LevelLoader.puzzleToLoad;
        }
        else
        {
            TextAsset selectedPuzzle = savedPuzzleFiles[puzzleIndex];
            string json = selectedPuzzle.text;
            NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(json);

            return loadedPuzzle;
        }
    }

    void GeneratePuzzle()
    {
        rows = puzzle.Rows;
        columns = puzzle.Cols;

        if (puzzle.timer > 0)
        { TimerScript.instance.BeginTimer(puzzle.timer); }
        else
        { TimerScript.instance.BeginTimer(0); }


        gridParent.GetComponent<GridLayoutGroup>().constraintCount = columns;

        // Clear existing content
        ClearBoard();

        // Create row clues
        GenerateRowClues();

        // Create column clues
        GenerateColumnClues();

        // Create grid
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

                switch (puzzle.GridData[r, c])
                {
                    case 0:
                        cellButton.State = CellState.Blank;
                        break;
                    case 1:
                        cellButton.State = CellState.Filled;
                        break;
                    case 2:
                        cellButton.State = CellState.Crossed;
                        break;
                }

                cellButton.UpdateVisuals();
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
                    // Puzzle not solved;
                    return;
                }
            }
        }

        // Game is won
        // Show win screen
        sounds.PlaySFX(sounds.completeSFX);

        // Find and set the solution sprite assigned to this puzzle
        for (int i=0; i < savedPuzzleFiles.Count; ++i)
        {
            if (puzzleIndex == i)
            {
                victoryScreenSprite = victorySprites[i];
                solutionScreen.SetSolutionScreen(victoryScreenSprite);
            }
        }
        victoryPanel.SetActive(true);
        TimerScript.instance.PauseTimer();
    }

    void BackToOverworld()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void SaveGame()
    {

        string fileName = matchPuzzle[puzzleIndex];

        puzzle.SaveProgress();
        puzzle.timer = TimerScript.instance.elapsedTime;

        string json = JsonUtility.ToJson(puzzle, true);
        System.IO.File.WriteAllText(saveProgressPath + fileName + ".json", json);

    }
}
