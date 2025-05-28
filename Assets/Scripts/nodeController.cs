using System.Collections;
using DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeController : MonoBehaviour
{
    public string sceneToLoad;
    [SerializeField] TextAsset linkedLevel;
    [SerializeField] GameObject linkedDialogueHolder;
    [SerializeField] private GameObject dialogueIntro;

    void OnMouseUpAsButton()
    {
        // if (!dialogueIntro.activeSelf)
        // {
        //     if (!string.IsNullOrEmpty(sceneToLoad))
        //     {
        //         linkedDialogueHolder.SetActive(true);
        //         loadLevel();
        //         StartCoroutine(WaitForAnimationAndChangeScene());
        //     }
        // }

        // More readable version of above code
        if (dialogueIntro.activeSelf) return;
        if (string.IsNullOrEmpty(sceneToLoad)) return;

        linkedDialogueHolder.SetActive(true);
        loadLevel();
        StartCoroutine(WaitForAnimationAndChangeScene());
    }

    void loadLevel()
    {
        NonogramPuzzle loadedPuzzle = JsonUtility.FromJson<NonogramPuzzle>(linkedLevel.text);
        LevelLoader.puzzleToLoad = loadedPuzzle;
        LevelLoader.puzzleName = linkedLevel.name;
    }

    private IEnumerator WaitForAnimationAndChangeScene()
    {
        // Wait while the linkedDialogueHolder is active
        while (linkedDialogueHolder.activeSelf)
        {
            yield return null; // Wait for the next frame
        }

        // Once linkedDialogueHolder is inactive, change the scene
        SceneController.instance.ChangeScene(sceneToLoad);
    }
}
