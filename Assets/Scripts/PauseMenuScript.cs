using UnityEngine;
using UnityEngine.UIElements;

public class PausedMenuScript : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject exitPanel;
    public GameObject optionPanel;
    public CanvasGroup pauseButton;

    private bool isPaused = false;

    //private void Awake()
    //{
        
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
        exitPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
