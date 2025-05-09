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

    public void ConfigureForStep(int stepIndex)
    {
        ButtonScript[] allCells = GetComponentsInChildren<ButtonScript>();

        foreach (var cell in allCells)
        {
            if (stepIndex == 3)
            {
                cell.EnableForTutorial(cell.col == 2);
            }
            else
            {
                cell.EnableForTutorial(false);
            }
        }
    }

}
