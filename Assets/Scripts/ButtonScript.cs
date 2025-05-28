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

    private Image targetImage;

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

        // Auto-load sprites from Resources if not assigned
        if (blankSprite == null)
            blankSprite = Resources.Load<Sprite>("nonogram_cell_unfilled");
        if (filledSprite == null)
            filledSprite = Resources.Load<Sprite>("nonogram_cell_filled");
        if (crossedSprite == null)
            crossedSprite = Resources.Load<Sprite>("nonogram_cell_crossed");
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (isPartOfTutorial)
        {
            if (!isTutorialInteractive)
            {
                Debug.Log($"Cell ({row}, {col}) is not interactive in tutorial.");
                return;
            }

            if (data.button == PointerEventData.InputButton.Left)
            {
                if (State == CellState.Blank)
                {
                    State = CellState.Filled;
                    puzzle.GridData[row, col] = 1;
                }
                else if (State == CellState.Filled)
                {
                    State = CellState.Crossed;
                    puzzle.GridData[row, col] = 2;
                }
                else if (State == CellState.Crossed)
                {
                    State = CellState.Blank;
                    puzzle.GridData[row, col] = 0;
                }

                UpdateVisuals();
                Object.FindFirstObjectByType<TutorialManager>()?.TryAutoAdvanceAfterInteraction();
                return;
            }

            if (data.button == PointerEventData.InputButton.Right)
            {
                if (State != CellState.Crossed)
                {
                    State = CellState.Crossed;
                    puzzle.GridData[row, col] = 2;
                }
                else
                {
                    State = CellState.Blank;
                    puzzle.GridData[row, col] = 0;
                }

                UpdateVisuals();
                Object.FindFirstObjectByType<TutorialManager>()?.TryAutoAdvanceAfterInteraction();
                return;
            }

            return;
        }

        // Non-tutorial behavior
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
        Debug.Log($"[Tutorial] Toggling state at ({row},{col}) from {State}");

        switch (State)
        {
            case CellState.Blank:
                State = CellState.Filled;
                puzzle.GridData[row, col] = 1;
                break;
            case CellState.Filled:
                State = CellState.Crossed;
                puzzle.GridData[row, col] = 2;
                break;
            case CellState.Crossed:
                State = CellState.Blank;
                puzzle.GridData[row, col] = 0;
                break;
        }

        UpdateVisuals();

        Object.FindFirstObjectByType<TutorialManager>()?.TryAutoAdvanceAfterInteraction();
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
        Image image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogWarning($"[Visual] No Image component found on cell ({row},{col})!");
            return;
        }

        switch (State)
        {
            case CellState.Blank:
                image.sprite = blankSprite;
                break;
            case CellState.Filled:
                image.sprite = filledSprite;
                break;
            case CellState.Crossed:
                image.sprite = crossedSprite;
                break;
        }
    }
}
