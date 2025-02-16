using UnityEngine;
using UnityEngine.UI;

public enum CellState {Unknown, Blank, Filled }
public class ButtonScript : MonoBehaviour
{
    public CellState State { get; private set; } = CellState.Blank;
    [SerializeField] Sprite unknownSprite, blankSprite, filledSprite;

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

    private void UpdateVisuals()
    {
        switch (State)
        {
            case CellState.Blank: GetComponent<Button>().image.sprite = blankSprite; break;
            case CellState.Filled: GetComponent<Button>().image.sprite = filledSprite; break;
            case CellState.Unknown: GetComponent<Button>().image.sprite = unknownSprite; break;
        }
    }
}
