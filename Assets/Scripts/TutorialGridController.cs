using System.Collections.Generic;
using UnityEngine;

public class TutorialGridController : MonoBehaviour
{
    [Header("Grid & Puzzle")]
    public GameObject puzzleContainer;
    public NonogramPuzzle puzzle; 
    public void ConfigureForStep(int step)
    {
        ButtonScript[] allCells = puzzleContainer.GetComponentsInChildren<ButtonScript>();

        foreach (ButtonScript cell in allCells)
        {
            bool enableCell = false;

            switch (step)
            {
                case 3:
                    enableCell = cell.col == 2;
                    break;
                case 5:
                case 6:
                    enableCell = cell.col == 0;
                    break;
                case 8:
                    enableCell = cell.row == 2;
                    break;
                case 9:
                    enableCell = cell.row == 0;
                    break;
                case 10:
                    enableCell = true;
                    break;
                default:
                    enableCell = false;
                    break;
            }

            cell.isPartOfTutorial = true;
            cell.EnableForTutorial(enableCell);
        }
    }
    
    public class TutorialStepGoal
    {
        public List<Vector2Int> RequiredFilledCells = new List<Vector2Int>();
        public List<Vector2Int> RequiredCrossedCells = new List<Vector2Int>();
    }

    private Dictionary<int, TutorialStepGoal> stepGoals = new Dictionary<int, TutorialStepGoal>
    {
        { 3, new TutorialStepGoal {
            RequiredFilledCells = new List<Vector2Int> {
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
                new Vector2Int(4, 2),
                new Vector2Int(5, 2)
            }
        }},
        { 5, new TutorialStepGoal {
            RequiredFilledCells = new List<Vector2Int> {
                new Vector2Int(0, 0),
                new Vector2Int(2, 0),
                new Vector2Int(4, 0),
                new Vector2Int(5, 0),
            }
        }},
        { 6, new TutorialStepGoal {
            RequiredCrossedCells = new List<Vector2Int> {
                new Vector2Int(1, 0),
                new Vector2Int(3, 0)
            }
        }},
        { 8, new TutorialStepGoal {
            RequiredCrossedCells = new List<Vector2Int> {
                new Vector2Int(2, 1),
                new Vector2Int(2, 3)
            }
        }},
        { 9, new TutorialStepGoal {
            RequiredFilledCells = new List<Vector2Int> {
                new Vector2Int(0, 1), 
            }
        }},
        { 10, new TutorialStepGoal {
            RequiredFilledCells = new List<Vector2Int> {
                new Vector2Int(1,3),
                new Vector2Int(3,1),
                new Vector2Int(4,1),
            }
        }}
    };

    public bool IsCurrentStepComplete(int currentStep)
    {
        if (!stepGoals.ContainsKey(currentStep) || puzzle == null)
        {
            return false;
        }

        var goal = stepGoals[currentStep];

        foreach (var cell in goal.RequiredFilledCells)
        {
            if (puzzle.GridData[cell.x, cell.y] != 1)
                return false;
        }

        foreach (var cell in goal.RequiredCrossedCells)
        {
            if (puzzle.GridData[cell.x, cell.y] != 2)
                return false;
        }

        return true;
    }
}
