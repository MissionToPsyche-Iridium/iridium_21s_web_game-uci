using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField] TextAsset linkedLevel;

    void OnMouseUpAsButton()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            loadLevel();
            //SceneManager.LoadScene(sceneToLoad);
            SceneController.instance.ChangeScene(sceneToLoad);
        }
    }

    void loadLevel()
    {
        NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(linkedLevel.text);
        LevelLoader.puzzleToLoad = loadedPuzzle;
        LevelLoader.puzzleName = linkedLevel.name;
    }
}
