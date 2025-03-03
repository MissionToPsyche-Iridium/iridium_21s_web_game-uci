using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CellState { Crossed, Blank, Filled }
public class ButtonScript : MonoBehaviour
{
    public static ButtonScript instance;
    public CellState State { get; set; } = CellState.Blank;
    [SerializeField] Sprite crossedSprite, blankSprite, filledSprite;

    [HideInInspector] public int row, col;
    //Connection to puzzle
    [HideInInspector] public NonogramPuzzle puzzle;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeGeneratorState()//Called from the button
    {
        if (State == CellState.Blank)
        {
            State = CellState.Filled;
        }
        else
        {
            State = CellState.Blank;
        }

        puzzle.SolutionData[row, col] = puzzle.SolutionData[row, col] == 1 ? 0 : 1;

        UpdateVisuals();
        //Notify GridManager
        GridManager.instance.OnCellStateChanged();
    }

    public void ChangeGameState()
    {
        int prevState = puzzle.GridData[row, col];
        UIUndoRedo.instance.undoActions.Push(new UIUndoRedo.UserAction(row, col, prevState));
        UIUndoRedo.instance.redoActions.Clear();
        Debug.Log($"undoActions count is {UIUndoRedo.instance.undoActions.Count}");

        if (State == CellState.Blank)
        {
            puzzle.GridData[row, col] = 1;
            State = CellState.Filled;
        }
        else if (State == CellState.Filled)
        {
            puzzle.GridData[row, col] = 2;
            State = CellState.Crossed;
        }
        else
        {
            puzzle.GridData[row, col] = 0;
            State = CellState.Blank;
        }
        UpdateVisuals();
        //Check for win condition
        GameManager.Instance.CheckWinCondition();
    }

    public void UndoCell()
    {
        Debug.Log($"row and col is {row} and {col}");
        if (State == CellState.Blank)
        {
            State = CellState.Crossed;
        }
        else if (State == CellState.Filled)
        {
            State = CellState.Blank;
        }
        else
        {
            State = CellState.Filled;
        }
        UpdateVisuals();
    }

    public void RedoCell()
    {
        Debug.Log($"row and col is {row} and {col}");
        if (State == CellState.Blank)
        {
            State = CellState.Filled;
        }
        else if (State == CellState.Filled)
        {
            State = CellState.Crossed;
        }
        else
        {
            State = CellState.Blank;
        }
        UpdateVisuals();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateVisuals()
    {
        switch (State)
        {
            case CellState.Blank: GetComponent<Button>().image.sprite = blankSprite; break;
            case CellState.Filled: GetComponent<Button>().image.sprite = filledSprite; break;
            case CellState.Crossed: GetComponent<Button>().image.sprite = crossedSprite; break;
        }
    }
}
