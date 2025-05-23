using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CellState { Crossed, Blank, Filled }
public class ButtonScript : MonoBehaviour, IPointerClickHandler
{
    public static ButtonScript instance;
    AudioManager sounds;
    public CellState State { get; set; } = CellState.Blank;
    [SerializeField] Sprite crossedSprite, blankSprite, filledSprite;

    [HideInInspector] public int row, col;
    // Connection to puzzle
    [HideInInspector] public NonogramPuzzle puzzle;

    private void Awake()
    {
        instance = this;
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void ChangeGeneratorState()  // Called from the button
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
        sounds.PlaySFX(sounds.gridTileSFX);

        UIUndoRedo.instance.undoActions.Push(this.gameObject);
        UIUndoRedo.instance.redoActions.Clear();

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
        // Check for win condition
        GameManager.Instance.CheckWinCondition();
    }

    public void UndoCell()
    {
        if (State == CellState.Blank)
        {
            puzzle.GridData[row, col] = 2;
            State = CellState.Crossed;
        }
        else if (State == CellState.Filled)
        {
            puzzle.GridData[row, col] = 0;
            State = CellState.Blank;
        }
        else
        {
            if (puzzle.GridData[row, col] == 3)
            {
                puzzle.GridData[row, col] = 4;
                State = CellState.Blank;
            }
            else
            {
                puzzle.GridData[row, col] = 1;
                State = CellState.Filled;
            }
        }
        UpdateVisuals();
        GameManager.Instance.CheckWinCondition();
    }

    public void RedoCell()
    {
        if (puzzle.GridData[row, col] == 4)
        {
            puzzle.GridData[row, col] = 3;
            State = CellState.Crossed;
        }
        else if (State == CellState.Blank)
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
        GameManager.Instance.CheckWinCondition();
    }

    public void ClearCell()
    {
        if (State != CellState.Blank)
        {
            puzzle.GridData[row, col] = 0;
            State = CellState.Blank;
            UpdateVisuals();
        }
    }

    public void Restart()
    {
        sounds.PlaySFX(sounds.restartSFX);

        UIUndoRedo.instance.redoActions.Clear();

        while (UIUndoRedo.instance.undoActions.Count > 0)
        {
            GameObject objectPresent = UIUndoRedo.instance.undoActions.Peek();
            if (objectPresent != null)
            {
                ButtonScript buttonScript = objectPresent.GetComponent<ButtonScript>();
                if (buttonScript != null)
                {
                    buttonScript.ClearCell();
                }
            }

            UIUndoRedo.instance.undoActions.Pop();
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Right)
        {
            sounds.PlaySFX(sounds.gridTileSFX);

            UIUndoRedo.instance.undoActions.Push(this.gameObject);
            UIUndoRedo.instance.redoActions.Clear();

            puzzle.GridData[row, col] = State == CellState.Filled ? puzzle.GridData[row, col] = 2 
                : puzzle.GridData[row, col] = 3;
            State = CellState.Crossed;
            UpdateVisuals();
            GameManager.Instance.CheckWinCondition();
        }
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