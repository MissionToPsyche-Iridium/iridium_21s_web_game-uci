using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CellState { Crossed, Blank, Filled }

public class ButtonScript : MonoBehaviour, IPointerClickHandler
{
    public static ButtonScript instance;

    [Header("Sprites")]
    [SerializeField] private Sprite crossedSprite;
    [SerializeField] private Sprite blankSprite;
    [SerializeField] private Sprite filledSprite;

    [HideInInspector] public int row, col;
    [HideInInspector] public NonogramPuzzle puzzle;

    private AudioManager sounds;

    public CellState State { get; set; } = CellState.Blank;

    [Header("Tutorial Settings")]
    public bool isPartOfTutorial = false;
    public bool isTutorialInteractive = false;

    private void Awake()
    {
        instance = this;
        sounds = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log($"Cell clicked: ({row}, {col})");

        if (isPartOfTutorial)
        {
            if (isTutorialInteractive && data.button == PointerEventData.InputButton.Left)
            {
                ToggleTutorialState();
            }
            return;
        }

        if (data.button == PointerEventData.InputButton.Left)
        {
            ChangeGameState();
        }
        else if (data.button == PointerEventData.InputButton.Right)
        {
            ToggleCrossedState();
        }
    }

    private void ToggleTutorialState()
    {
        if (State == CellState.Blank)
        {
            State = CellState.Filled;
        }
        else
        {
            State = CellState.Blank;
        }
        UpdateVisuals();
    }

    public void ChangeGameState()
    {
        sounds?.PlaySFX(sounds.gridTileSFX);

        UIUndoRedo.instance?.undoActions.Push(gameObject);
        UIUndoRedo.instance?.redoActions.Clear();

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
        GameManager.Instance?.CheckWinCondition();
    }

    private void ToggleCrossedState()
    {
        sounds?.PlaySFX(sounds.gridTileSFX);

        UIUndoRedo.instance?.undoActions.Push(gameObject);
        UIUndoRedo.instance?.redoActions.Clear();

        puzzle.GridData[row, col] = 3;
        State = CellState.Crossed;

        UpdateVisuals();
        GameManager.Instance?.CheckWinCondition();
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
            puzzle.GridData[row, col] = 1;
            State = CellState.Filled;
        }
        UpdateVisuals();
        GameManager.Instance.CheckWinCondition();
    }

    public void RedoCell()
    {
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
        sounds?.PlaySFX(sounds.restartSFX);

        UIUndoRedo.instance?.redoActions.Clear();

        while (UIUndoRedo.instance?.undoActions.Count > 0)
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

    public void EnableForTutorial(bool isEnabled)
    {
        isTutorialInteractive = isEnabled;

        if (TryGetComponent(out Button uiButton))
        {
            uiButton.interactable = isEnabled;
        }
    }

    public void UpdateVisuals()
    {
        if (TryGetComponent(out Button uiButton))
        {
            switch (State)
            {
                case CellState.Blank:
                    uiButton.image.sprite = blankSprite;
                    break;
                case CellState.Filled:
                    uiButton.image.sprite = filledSprite;
                    break;
                case CellState.Crossed:
                    uiButton.image.sprite = crossedSprite;
                    break;
            }
        }
    }
}
