using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField] TextAsset linkedLevel;
    [SerializeField] GameObject linkedDialogHolder;

    void OnMouseUpAsButton()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            linkedDialogHolder.SetActive(true);
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
