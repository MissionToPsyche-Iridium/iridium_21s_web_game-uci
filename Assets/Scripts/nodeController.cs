using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField] TextAsset linkedLevel;

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            loadLevel();
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void loadLevel()
    {
        NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(linkedLevel.text);
        LevelLoader.puzzleToLoad = loadedPuzzle;
        LevelLoader.puzzleName = linkedLevel.name;
    }
}
