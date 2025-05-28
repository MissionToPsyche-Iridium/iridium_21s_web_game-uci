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

    private readonly HashSet<int> interactiveSteps = new HashSet<int> { 3, 5, 6, 8, 9, 10};


    private readonly string[] stepTitles = new string[]
    {
        "Understanding the grid",
        "The Clues",
        "Column Clues",
        "Let's practice",
        "Column Clues",
        "Let's practice",
        "Marking White Cells",
        "Row Clues",
        "Let's Practice",   
        "Let's Practice",
        "Final Step!"
    };

    private readonly string[] stepDescriptions = new string[]
    {
        "The goal is to uncover a hidden picture by following numerical clues.",
        "Clues on top tell you how many filled cells are within the column.",
        "Since this is a 4x6 grid, the max amount of column grid is 6.\n\nSo, we know this entire column is filled.",
        "Now, click on the cells to fill them in.",
        "When a clue shows more than one number, each group of filled squares must be kept apart by at least one empty square.",
        "The first cell is filled for you. \nTry to fill the rest out!",
        "Nice job!\n\n Now, click on the empty cells twice or right click to mark it with an 'X'. These are known white cells.",
        "Let's learn about the rows. \n\nClues on the side tell you how many filled cells are within each row.",
        "Notice that this row was automatically completed as soon as the intersecting column was filled.\n\nLet's mark the remaining cells with an 'X'!",
        "This row requires 3 filled squares in a row.\n\nFill in the remaining squares to complete it.",
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

        continueStepsButton.gameObject.SetActive(!interactiveSteps.Contains(currentStep));
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
            SceneController.instance.ChangeScene("NonogramGameScene");
            return;
        }

        ShowStep();
    }

    void SkipTutorial()
    {
        SceneController.instance.ChangeScene("NonogramGameScene");
    }

    public void TryAutoAdvanceAfterInteraction()
    {
        if (interactiveSteps.Contains(currentStep) &&
            gridController.IsCurrentStepComplete(currentStep))
        {
            NotifyPracticeComplete(); // reveal Continue
        }
    }


}
