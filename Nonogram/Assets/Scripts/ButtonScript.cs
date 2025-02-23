using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState {Crossed, Blank, Filled }
public class ButtonScript : MonoBehaviour
{
    private Stack<UserAction> undoActions = new Stack<UserAction>();
    private Stack<UserAction> redoActions = new Stack<UserAction>();
    public CellState State { get; private set; } = CellState.Blank;
    [SerializeField] Sprite crossedSprite, blankSprite, filledSprite;

    [HideInInspector]public int row, col;
    //Connection to puzzle
    [HideInInspector] public NonogramPuzzle puzzle;

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

        puzzle.SolutionData[row,col] = puzzle.SolutionData[row,col] == 1 ? 0 : 1;

        UpdateVisuals();
        //Notify GridManager
        GridManager.instance.OnCellStateChanged();
    }

    public void ChangeGameState()
    {
        int prevState = puzzle.GridData[row, col];
        undoActions.Push(new UserAction(row, col, prevState));
        redoActions.Clear();

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

    public void Undo()
    {
        if (undoActions.Count > 0)
        {
            UserAction prevAction = undoActions.Pop();
            int nextState = puzzle.GridData[prevAction.savedRow, prevAction.savedCol];

            redoActions.Push(new UserAction(prevAction.savedRow, prevAction.savedCol, nextState));
            puzzle.GridData[prevAction.savedRow, prevAction.savedCol] = prevAction.savedState;
            
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
    }

    public void Redo()
    {
        if (redoActions.Count > 0)
        {
            UserAction nextAction = redoActions.Pop();
            int nextState = puzzle.GridData[nextAction.savedRow, nextAction.savedCol];

            undoActions.Push(new UserAction(nextAction.savedRow, nextAction.savedCol, nextState));

            puzzle.GridData[nextAction.savedRow, nextAction.savedCol] = nextAction.savedState;
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
    }   

    // Detects Ctrl Z and Ctrl Y for Undo and Redo
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Ctrl Z kept redoing my asset changes, so I changed it to Ctrl E for now lol
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Undo went through");
                Undo();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("Redo went through");
                Redo();
            }
        }
    }

    private struct UserAction
    {
        public int savedRow, savedCol, savedState;
        public UserAction(int saveRow, int saveCol, int saveState)
        {
            savedRow = saveRow;
            savedCol = saveCol;
            savedState = saveState;
        }
    }

    private void UpdateVisuals()
    {
        switch (State)
        {
            case CellState.Blank: GetComponent<Button>().image.sprite = blankSprite; break;
            case CellState.Filled: GetComponent<Button>().image.sprite = filledSprite; break;
            case CellState.Crossed: GetComponent<Button>().image.sprite = crossedSprite; break;
        }
    }
}
