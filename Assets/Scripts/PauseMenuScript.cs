using UnityEngine;
using UnityEngine.UIElements;

public class PausedMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pausePanel, exitPanel, optionPanel;
    [SerializeField] CanvasGroup pauseButton;

    private bool isPaused = false;
    private string sceneToLoad = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
        exitPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void TogglePausePanel()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            pausePanel.SetActive(true);
            pauseButton.interactable = false;
            pauseButton.alpha = 0f;
        }
        else
        {

            pausePanel.SetActive(false);
            pauseButton.interactable = true;
            pauseButton.alpha = 1f;
        }
    }

    public void ToggleExitPanel()
    {
        pausePanel.SetActive(false);
        exitPanel.SetActive(true);
        exitPanel.GetComponent<ExitMenuScript>().SetExitType(sceneToLoad);
    }

    public void ExitToHome()
    {
        sceneToLoad = "StartMenu";
        pausePanel.SetActive(false);
        exitPanel.SetActive(true);
        ExitMenuScript.instance.SendMessage("SetExitType", sceneToLoad);
    }

    public void ExitToMap()
    {
        sceneToLoad = "MapScene";
        pausePanel.SetActive(false);
        exitPanel.SetActive(true);
        ExitMenuScript.instance.SendMessage("SetExitType", sceneToLoad);
    }
}
