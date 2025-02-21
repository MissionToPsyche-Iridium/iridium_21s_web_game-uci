using UnityEngine;
using UnityEngine.UI;

public enum CellState {Crossed, Blank, Filled }
public class ButtonScript : MonoBehaviour
{
    public CellState State { get; private set; } = CellState.Blank;
    [SerializeField] Sprite crossedSprite, blankSprite, filledSprite;

    [HideInInspector]public int row, col;
    //Connection to puzzle
    [HideInInspector] public NonogramPuzzle puzzle;

    public void ChangeGeneratorState()//Called from the button
    {
        if(State == CellState.Blank)
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
        if(State == CellState.Blank)
        {
            puzzle.GridData[row, col] = 1;
            State = CellState.Filled;
        }
        else if(State == CellState.Filled)
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
