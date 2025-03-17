using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject launchPanel;  // Reference to the Level 1 - Launch panel
    public Button continueToTutorialButton; // Button to start tutorial
    public Button skipTutorialButton; // Button to skip tutorial

    void Start()
    {
        // Show launch panel on game start
        launchPanel.SetActive(true);

        // Assign button events
        continueToTutorialButton.onClick.AddListener(StartTutorial);
        skipTutorialButton.onClick.AddListener(SkipTutorial);
    }

    void StartTutorial()
    {
        // Load the new tutorial scene
        SceneManager.LoadScene("TutorialScene");
    }

    void SkipTutorial()
    {
        // Close launch panel and return to game (or main map)
        launchPanel.SetActive(false);
    }
}
