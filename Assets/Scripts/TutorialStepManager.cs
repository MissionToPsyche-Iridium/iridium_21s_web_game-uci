using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialStepManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelPanel;
    public GameObject tutorialStepPanel;

    [Header("Intro Buttons")]
    public Button continueTutorialButton;
    public Button skipTutorialButton;

    [Header("Step UI")]
    public TextMeshProUGUI stepsTitleText;
    public TextMeshProUGUI descriptionText;
    public Button continueStepsButton;
    public GameObject[] highlightRects;

    [Header("Grid Control")]
    public TutorialGridController gridController;

    private int currentStep = 0;

    private string[] stepTitles = new string[]
    {
        "Understanding the grid",
        "The Clues",
        "More Clues",
        "Let's Practice",
        "Empty Squares",
        "Try It",
        "Finished!"
    };

    private string[] stepDescriptions = new string[]
    {
        "The goal is to uncover a hidden picture by following numerical clues.",
        "Clues on top tell you how many filled cells are within the column.",
        "Since this is a 4x6 grid, the max amount of column grid is 6. So, we know this entire column is filled.",
        "Now, click on the cells to fill them in.",
        "To mark empty cells, right click on them to place an X.",
        "Try marking the remaining cells now!",
        "Awesome! You're ready."
    };

    void Start()
    {
        levelPanel.SetActive(true);
        tutorialStepPanel.SetActive(false);

        continueTutorialButton.onClick.AddListener(StartTutorial);
        skipTutorialButton.onClick.AddListener(SkipTutorial);
        continueStepsButton.onClick.AddListener(NextStep);
    }

    void StartTutorial()
    {
        levelPanel.SetActive(false);
        tutorialStepPanel.SetActive(true);
        currentStep = 0;
        ShowCurrentStep();
    }

    void ShowCurrentStep()
    {
        stepsTitleText.text = stepTitles[currentStep];
        descriptionText.text = stepDescriptions[currentStep];

        for (int i = 0; i < highlightRects.Length; i++)
            highlightRects[i].SetActive(i == currentStep);

        if (currentStep == 3)
        {
            continueStepsButton.gameObject.SetActive(false);
            gridController.EnableColumn(2);
        }
        else if (currentStep == 5)
        {
            continueStepsButton.gameObject.SetActive(false);
            gridController.EnableAllCells();
        }
        else
        {
            continueStepsButton.gameObject.SetActive(true);
            gridController.DisableAllCells();
        }
    }

    public void NotifyPracticeComplete()
    {
        continueStepsButton.gameObject.SetActive(true);
    }

    void NextStep()
    {
        currentStep++;
        if (currentStep >= stepTitles.Length)
        {
            SceneManager.LoadScene("NonogramGameScene");
            return;
        }

        ShowCurrentStep();
    }

    void SkipTutorial()
    {
        SceneManager.LoadScene("NonogramGameScene");
    }
}