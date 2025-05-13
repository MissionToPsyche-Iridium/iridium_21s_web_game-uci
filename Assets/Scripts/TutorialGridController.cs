using UnityEngine;

public class TutorialGridController : MonoBehaviour
{
    public GameObject puzzleContainer;

    public void EnableColumn(int colIndex)
    {
        var allCells = puzzleContainer.GetComponentsInChildren<ButtonScript>();
        foreach (var cell in allCells)
        {
            cell.EnableForTutorial(cell.col == colIndex);
        }
    }

    public void DisableAllCells()
    {
        var allCells = puzzleContainer.GetComponentsInChildren<ButtonScript>();
        foreach (var cell in allCells)
        {
            cell.EnableForTutorial(false);
        }
    }

    public void EnableAllCells()
    {
        var allCells = puzzleContainer.GetComponentsInChildren<ButtonScript>();
        foreach (var cell in allCells)
        {
            cell.EnableForTutorial(true);
        }
    }

    public void ConfigureForStep(int step)
    {
        var allButtons = puzzleContainer.GetComponentsInChildren<ButtonScript>();

        foreach (var btn in allButtons)
        {
            if (btn == null) continue;

            if (step == 3)
            {
                btn.EnableForTutorial(btn.col == 2); // Only 3rd column (col = 2)
            }
            else
            {
                btn.EnableForTutorial(false); // Disable all interactivity
            }
        }
    }

}
