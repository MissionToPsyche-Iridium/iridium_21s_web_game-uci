using UnityEngine;
using System.IO;

public class TutorialPuzzleLoader : MonoBehaviour
{
    [SerializeField] private string puzzleName = "TutorialPuz";

    public void LoadTutorialPuzzle()
    {
        string path = Path.Combine(Application.dataPath, "SavedPuzzles", puzzleName + ".json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            NonogramPuzzle puzzle = JsonUtility.FromJson<NonogramPuzzle>(json);

            LevelLoader.puzzleName = puzzleName;
            LevelLoader.puzzleToLoad = puzzle;

            UnityEngine.SceneManagement.SceneManager.LoadScene("NonogramGameScene");
        }
        else
        {
            Debug.LogError("Tutorial puzzle not found at path: " + path);
        }
    }
}
