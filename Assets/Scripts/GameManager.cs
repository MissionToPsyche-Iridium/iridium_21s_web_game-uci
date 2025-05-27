using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] List<TextAsset> savedPuzzleFiles;
    [SerializeField] List<Sprite> victorySprites;
    [SerializeField] Transform gridParent, rowClueParent, colClueParent;
    [SerializeField] GameObject rowCluePrefab, colCluePrefab, cellButtonPrefab;

    [Header("Analyze")]
    [SerializeField] GameObject analyzePanel;
    [SerializeField] Button analyzeButton;
    [SerializeField] private Animator completedPuzzleToSolutionTransition;

    [Header("Win")]
    [SerializeField] GameObject victoryPanel;
    [SerializeField] Button victoryButton;
    Sprite victoryScreenSprite;
    public SolutionPanelScript solutionScreen;

    int puzzleIndex = 0;
    int rows, columns;
    public bool restartStopwatch = false;
    public float prevSolvedTime = 0f;

    AudioManager sounds;
    NonogramPuzzle puzzle;

    //string saveProgressPath = Application.dataPath + "/ProgressPuzzles/";

    Dictionary<int, string> matchPuzzle = new Dictionary<int, string>()
    {
        {0, "5x5Rocket"},
        {1, "5x5SpaceX"},
        {2, "5x5Sixteen"},
        {3, "5x5PsycheLogo"},
        {4, "6x6SatelliteDish"},
        {5, "6x6HallThruster"},
        {6, "6x6GravityAssist"},
        {7, "6x6Spacecraft"},
        {8, "8x8Belt"},
        {9, "8x8Spacecraft"},
        {10, "8x8OrbitAxes"},
        {11, "8x8Asteroid"},
        {12, "PsycheLogo"},
        {13, "Spacecraft"},
        {14, "Asteroid"},
        {15, "Instruments"}
    };

    Dictionary<int, GameObject> RowClueDictionary = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> ColClueDictionary = new Dictionary<int, GameObject>();

    int numPuzzlesSolved = 0;



    private void Awake()
    {
        Instance = this;
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }

    void Start()
    {
        analyzeButton.onClick.AddListener(OnAnalyzeButtonClicked);
        analyzePanel.SetActive(false);

        victoryButton.onClick.AddListener(BackToOverworld);
        victoryPanel.SetActive(false);

        SetPuzzleIndex();
        UpdateStageText();
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
        //PlayerPrefs.DeleteKey("SavedPuzzleIndex");
        //PlayerPrefs.DeleteKey("SavedPuzzle");
        //PlayerPrefs.DeleteKey("SavedTime");


        if (LevelLoader.puzzleToLoad != null)
        {
            // If a puzzle is already loaded, use it
            if (PlayerPrefs.HasKey("SavedPuzzleIndex"))
            {
                puzzleIndex = PlayerPrefs.GetInt("SavedPuzzleIndex");
            }

            // If a puzzle is saved, load it
            if (PlayerPrefs.HasKey("SavedPuzzle") && 
                PlayerPrefs.GetInt("SavedPuzzleIndex") == puzzleIndex)
            { 
                string puzzleJson = PlayerPrefs.GetString("SavedPuzzle");
                PlayerPrefs.SetString("SavedPuzzle", string.Empty);
                NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(puzzleJson);
                return loadedPuzzle;
            }

            // Assign an index number to each puzzle by name
            puzzleIndex = matchPuzzle.FirstOrDefault(x => x.Value == LevelLoader.puzzleName).Key;

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
        float checkTime = TimerScript.instance.elapsedTime;
        bool checkSavedTime = PlayerPrefs.HasKey("SavedTime") &&
            PlayerPrefs.GetFloat("SavedTime")>0 &&
            PlayerPrefs.GetInt("SavedPuzzleIndex") == puzzleIndex;

        if (checkSavedTime)
        {
            TimerScript.instance.RestartTimer(PlayerPrefs.GetFloat("SavedTime"));
            PlayerPrefs.SetFloat("SavedTime", 0f);
        }
        else if (checkTime > 0 && restartStopwatch)
        { 
            TimerScript.instance.RestartTimer(prevSolvedTime);
            restartStopwatch = false;
        }
        else if (checkTime == 0 && !restartStopwatch)
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
            RowClueDictionary[r] = rowClue;
        }
    }

    void GenerateColumnClues()
    {
        for (int c = 0; c < columns; c++)
        {
            GameObject colClue = Instantiate(colCluePrefab, colClueParent);
            colClue.GetComponentInChildren<TMP_Text>().text = string.Join("\n", puzzle.ColClues[c].Clues);
            ColClueDictionary[c] = colClue;
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
        bool puzzleSolved = true;

        //check rows
        for (int r = 0; r < puzzle.Rows; r++)
        {
            if(CheckRow(r))
            {
                if(RowClueDictionary[r].GetComponentInChildren<TMP_Text>().color != Color.grey)
                {
                    RowClueDictionary[r].GetComponentInChildren<TMP_Text>().color = Color.grey;
                    sounds.PlaySFX(sounds.clueSolvedSFX);
                }
                
            }
            else
            {
                puzzleSolved = false;
                if(RowClueDictionary[r].GetComponentInChildren<TMP_Text>().color == Color.grey)
                {
                    RowClueDictionary[r].GetComponentInChildren<TMP_Text>().color = Color.black;
                    sounds.PlaySFX(sounds.clueUnsolvedSFX);
                }
            }
        }

        //check columns
        for (int c = 0; c < puzzle.Cols; c++)
        {
            if (CheckColumn(c))
            {
                if (ColClueDictionary[c].GetComponentInChildren<TMP_Text>().color != Color.grey)
                {
                    ColClueDictionary[c].GetComponentInChildren<TMP_Text>().color = Color.grey;
                    sounds.PlaySFX(sounds.clueSolvedSFX);
                }
                
            }
            else
            {
                puzzleSolved = false;
                if (ColClueDictionary[c].GetComponentInChildren<TMP_Text>().color == Color.grey)
                {
                    ColClueDictionary[c].GetComponentInChildren<TMP_Text>().color = Color.black;
                    sounds.PlaySFX(sounds.clueUnsolvedSFX);
                }
                
            }
        }

        if (puzzleSolved)
        {
            // Game is won
            // Show win screen
            sounds.PlaySFX(sounds.completeSFX);
            ++numPuzzlesSolved;
            prevSolvedTime = TimerScript.instance.elapsedTime;
            PlayerPrefs.DeleteKey("SavedPuzzle");
            PlayerPrefs.DeleteKey("SavedTime");

            // Find and set the solution sprite assigned to this puzzle
            for (int i = 0; i < savedPuzzleFiles.Count; ++i)
            {
                if (puzzleIndex/4 == i)
                {
                    victoryScreenSprite = victorySprites[i];
                    solutionScreen.SetSolutionScreen(victoryScreenSprite);
                }
            }
            if (numPuzzlesSolved % 4 == 0)
            {
                PlayerPrefs.DeleteKey("SavedPuzzleIndex");
            }
            if (numPuzzlesSolved > 3)
            {
                TimerScript.instance.PauseTimer();
                PlayerPrefs.SetInt("MaxCurrentLevel", (puzzleIndex + 1)/4);

                // Unity coroutines allow time-based delays
                StartCoroutine(ShowAnalyzePanelAfterPuzzleSolved());
            }
            else
            {
                TextAsset nextPuzzle = Resources.Load<TextAsset>("SavedPuzzles/"+ matchPuzzle[puzzleIndex + 1]);
                NonogramPuzzle newPuzzle = JsonUtility.FromJson<NonogramPuzzle>(nextPuzzle.text);
                LevelLoader.puzzleToLoad = newPuzzle;
                LevelLoader.puzzleName = nextPuzzle.name;
                LoadCurrentPuzzle();
                UpdateStageText();
            }

                
        }
        
    }

    bool CheckRow(int row)
    {
        for (int col = 0; col < puzzle.Cols; col++)
        {
            if (puzzle.SolutionData[row, col] == 1 && puzzle.GridData[row, col] != 1 ||
                    puzzle.SolutionData[row, col] == 0 && puzzle.GridData[row, col] == 1)
            {
                // Row not solved;
                return false;
            }
        }
        return true;
    }

    bool CheckColumn(int col)
    {
        for (int row = 0; row < puzzle.Rows; row++)
        {
            if (puzzle.SolutionData[row, col] == 1 && puzzle.GridData[row, col] != 1 ||
                    puzzle.SolutionData[row, col] == 0 && puzzle.GridData[row, col] == 1)
            {
                // Column not solved;
                return false;
            }
        }
        return true;
    }

    void BackToOverworld()
    {
        SceneController.instance.ChangeScene("MapScene");
    }

    public void SaveGame()
    {
        // Save the current progress to player prefs (local storage)
        PlayerPrefs.SetInt("SavedPuzzleIndex", puzzleIndex);
        Debug.Log(puzzleIndex);

        puzzle.SaveProgress();
        PlayerPrefs.SetFloat("SavedTime", TimerScript.instance.elapsedTime);
        string puzzleJson = JsonUtility.ToJson(puzzle, true);
        PlayerPrefs.SetString("SavedPuzzle", puzzleJson);

        PlayerPrefs.Save();
        //puzzle.skipTutorial = TutorialManager.instance.skipLaunchPanel;

        //string json = JsonUtility.ToJson(puzzle, true);
        //System.IO.File.WriteAllText(saveProgressPath + "test.json", json);
    }

    public void RestartProgress()
    {
        // Restart the progress and regenerate the puzzle
        puzzle.GridData = new int[rows, columns];
        puzzle.SaveProgress();
        restartStopwatch = true;
        GeneratePuzzle();
    }

    private void OnAnalyzeButtonClicked()
    {
        StartCoroutine(ShowSolutionPanelAfterAnalyzing());
    }

    private IEnumerator ShowAnalyzePanelAfterPuzzleSolved()
    {
        completedPuzzleToSolutionTransition.SetTrigger("isPuzzleSolved");


        yield return new WaitForSeconds(5.0f);

        analyzePanel.SetActive(true);
    }


    private IEnumerator ShowSolutionPanelAfterAnalyzing()
    {
        completedPuzzleToSolutionTransition.SetTrigger("isAnalyzing");


        yield return new WaitForSeconds(5.0f);

        analyzePanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    void UpdateStageText()
    {
        GameObject.Find("StageText").GetComponentInChildren<TMP_Text>().text = "Stage " + (puzzleIndex +1);
    }
}
