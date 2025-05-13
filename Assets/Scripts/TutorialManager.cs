using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject levelPanel;
    public GameObject tutorialStepPanel;

    [Header("Buttons")]
    public Button continueTutorialButton;
    public Button skipTutorialButton;
    public Button continueStepsButton;

    [Header("Step UI")]
    public TextMeshProUGUI stepsTitleText;
    public TextMeshProUGUI descriptionText;

    [Header("Controllers")]
    public TutorialHighlightController highlightController;
    public TutorialGridController gridController;

    private int currentStep = 0;

    private readonly string[] stepTitles = new string[]
    {
        "Understanding the grid",
        "The Clues",
        "The Clues",
        "Let's practice",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "The Clues",
        "Congrats!"
    };

    private readonly string[] stepDescriptions = new string[]
    {
        "The goal is to uncover a hidden picture by following numerical clues.",
        "Clues on top tell you how many filled cells are within the column.",
        "Since this is a 4x6 grid, the max amount of column grid is 6.\nSo, we know this entire column is filled.",
        "Now, click on the cells to fill them in.",
        "Nice job!",
        "When a clue shows more than one number, each group of filled squares must be kept apart by at least one empty square.",
        "The first cell is filled for you. Try to fill the rest out!",
        "Nice job! Now, click on the empty cells to mark it with an 'X'. These are known white cells.",
        "Nice job!",
        "Let's learn about the rows. Clues on the side tell you how many filled cells are within each row.",
        "Notice that this row was automatically completed as soon as the intersecting column was filled. Let's mark the remaining cells with an 'X'!",
        "Nice job!",
        "This row requires 3 filled squares in a row.\nFill in the remaining squares to complete it.",
        "Nice job!",
        "Now, finish the rest of the nonogram with what you've just learned!"
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
        ShowStep();
    }

    void ShowStep()
    {
        stepsTitleText.text = stepTitles[currentStep];
        descriptionText.text = stepDescriptions[currentStep];

        highlightController?.ShowHighlightForStep(currentStep);
        gridController?.ConfigureForStep(currentStep);

        continueStepsButton.gameObject.SetActive(true);
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

        ShowStep();
    }

    void SkipTutorial()
    {
        SceneManager.LoadScene("NonogramGameScene");
    }
}
