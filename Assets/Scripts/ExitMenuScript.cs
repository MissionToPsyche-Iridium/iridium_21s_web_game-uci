using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMenuScript : MonoBehaviour
{
    public static ExitMenuScript instance;
    [SerializeField] GameObject pausePanel, exitPanel;

    private string sceneToLoad = "";

    private void Awake()
    {
        instance = this;
    }


    public void SetExitScene(string destination)
    {
        sceneToLoad = destination;
    }

    public void OnYes()
    {
        // Save the game, exit to main menu or map
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnNo()
    {
        // Don't save the game, exit to main menu or map
        Debug.Log(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCancel()
    {
        exitPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}

