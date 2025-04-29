using UnityEngine;
using UnityEngine.UIElements;

public class PausedMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pausePanel, exitPanel, optionPanel;
    [SerializeField] CanvasGroup pauseButton;

    private bool isPaused = false;
    private string sceneToLoad = "";
    AudioManager sounds;

    private void Awake()
    {
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
        exitPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void TogglePausePanel()
    {
        // Toggle the pause menu, pause the game and activate the pause button
        sounds.PlaySFX(sounds.pauseSFX);
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

    public void ToggleOptionPanel()
    {
        // Toggle the option panel, disabling the pause menu
        sounds.PlaySFX(sounds.generalUIButton);
        pausePanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void ReturnPausePanel()
    {
        // Return to the pause menu
        sounds.PlaySFX(sounds.generalUIButton);
        pausePanel.SetActive(true);
        optionPanel.SetActive(false);
    }

    public void ExitToHome()
    {
        // Exit to the home menu
        sounds.PlaySFX(sounds.generalUIButton);
        sceneToLoad = "StartMenu";
        pausePanel.SetActive(false);
        exitPanel.SetActive(true);
        ExitMenuScript.instance.SendMessage("SetExitScene", sceneToLoad);
    }

    public void ExitToMap()
    {
        // Exit to the map menu
        sounds.PlaySFX(sounds.generalUIButton);
        sceneToLoad = "MapScene";
        pausePanel.SetActive(false);
        exitPanel.SetActive(true);
        ExitMenuScript.instance.SendMessage("SetExitScene", sceneToLoad);
    }
}
