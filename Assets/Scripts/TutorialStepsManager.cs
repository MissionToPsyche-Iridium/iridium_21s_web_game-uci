using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialStepsManager : MonoBehaviour
{
    [Header("Tutorial UI")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button continueButton;
    public Button playButton;

    [Header("Static Images for Steps")]
    public Image tutorialImage;
    public Sprite imageStep1;
    public Sprite imageStep2; 
    public Sprite imageStep3; 
    public Sprite imageStep4; 
    public Sprite imageStep5;
    public Sprite imageStep6;

    [Header("Interactive Grid Cells for Steps 3 & 4")]
    public Image interactiveGridCellStep3;
    public Image interactiveGridCellStep4;
    public Sprite emptyCellSprite;
    public Sprite filledCellSprite;
    public Sprite markedCellSprite;

    private int stepIndex = 0;
    private int cellClickCountStep3 = 0;
    private int cellClickCountStep4 = 0;

    private string[] tutorialTitles = {
        "UNDERSTANDING THE GRID",
        "THE CLUES",
        "FILLING SQUARES",
        "MARKING EMPTY SQUARES",
        "UNCOVERING THE PICTURE",
        "COMPLETING THE PUZZLE"
    };

    private string[] tutorialDescriptions = {
        "You'll be working with a grid.\nEach row and column has a set of numbers that tell you how many consecutive filled squares are in that line.",
        "Each number represents a group of filled-in squares in that row or column.\nFor example:\n\n5\n2\n\nmeans a group of 5 filled squares followed by a group of 2, with at least one empty space between them.",
        "To start, click on a square to fill it in.",
        "If you think a square should stay empty, mark it with an X.",
        "As you progress, the filled-in squares will start to reveal a picture.\n\nKeep using the clues to uncover more of the image and complete the puzzle!",
        "When all the squares are filled in correctly, you'll complete the picture and win the puzzle!\n\nCongratulations!\n\nThat's all! Ready to play?"
    };

    void Start()
    {
        stepIndex = 0;
        UpdateTutorialStep();

        continueButton.onClick.AddListener(NextStep);
        playButton.onClick.AddListener(StartGame);

        interactiveGridCellStep3.GetComponent<Button>().onClick.AddListener(OnCellClickedStep3);
        interactiveGridCellStep4.GetComponent<Button>().onClick.AddListener(OnCellClickedStep4);

        interactiveGridCellStep3.gameObject.SetActive(false);
        interactiveGridCellStep4.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    void UpdateTutorialStep()
    {
        if (stepIndex < tutorialTitles.Length)
        {
            titleText.text = tutorialTitles[stepIndex];
            descriptionText.text = tutorialDescriptions[stepIndex];

            // Set tutorial images based on step
            if (stepIndex == 0)
                tutorialImage.sprite = imageStep1; // Step 1 Image
            else if (stepIndex == 1)
                tutorialImage.sprite = imageStep2; // Step 2,
             else if (stepIndex == 2)
                tutorialImage.sprite = imageStep3; // Step 5 Image
             else if (stepIndex == 3)
                tutorialImage.sprite = imageStep4; // Step 5 Image
            else if (stepIndex == 4)
                tutorialImage.sprite = imageStep5; // Step 5 Image
            else if (stepIndex == 5)
                tutorialImage.sprite = imageStep6; // Step 6 Image

            // Step 3: Show interactive cell
            if (stepIndex == 2)
            {
                interactiveGridCellStep3.gameObject.SetActive(true);
                interactiveGridCellStep3.sprite = emptyCellSprite;
                cellClickCountStep3 = 0;
            }

            // Step 4: Keep Step 3 cell marked and introduce new cell
            if (stepIndex == 3)
            {
                interactiveGridCellStep3.gameObject.SetActive(false);
                interactiveGridCellStep4.gameObject.SetActive(true);
                interactiveGridCellStep4.sprite = emptyCellSprite;
                cellClickCountStep4 = 0;
            }

            // Step 5 & 6: Hide interactive cells
            if (stepIndex >= 4)
            {
                
                interactiveGridCellStep4.gameObject.SetActive(false);
            }

            // Step 6: Show Play button instead of Continue
            if (stepIndex == 5)
            {
                continueButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(true);
            }
            else
            {
                continueButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
            }
        }
        else
        {
            EndTutorial();
        }
    }

    void OnCellClickedStep3()
    {
        if (stepIndex == 2) 
        {
            if (cellClickCountStep3 == 0)
            {
                interactiveGridCellStep3.sprite = filledCellSprite;
                descriptionText.text = "Nice job!";
                cellClickCountStep3++;
            }
        }
    }

    void OnCellClickedStep4()
    {
        if (stepIndex == 3) 
        {
            if (cellClickCountStep4 == 0)
            {
                interactiveGridCellStep4.sprite = filledCellSprite;
                descriptionText.text = "Click again to mark it as empty.";
                cellClickCountStep4++;
            }
            else if (cellClickCountStep4 == 1)
            {
                interactiveGridCellStep4.sprite = markedCellSprite;
                descriptionText.text = "Nice job!";
                cellClickCountStep4++;
            }
        }
    }

    public void NextStep()
    {
        if ((stepIndex == 2 && cellClickCountStep3 == 0) || (stepIndex == 3 && cellClickCountStep4 < 2))
        {
            return; // Prevent moving forward until clicked
        }

        stepIndex++;
        UpdateTutorialStep();
    }

    void StartGame()
    {
        SceneManager.LoadScene("NonogramGameScene"); // Replace with your actual Nonogram game scene name
    }

    void EndTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}
