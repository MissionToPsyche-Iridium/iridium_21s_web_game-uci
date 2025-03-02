using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUndoRedo : MonoBehaviour
{
    public static UIUndoRedo instance;
    public Stack<UserAction> undoActions = new Stack<UserAction>();
    public Stack<UserAction> redoActions = new Stack<UserAction>();

    private void Awake()
    {
        instance = this;
    }

    public void Undo()
    {
        Debug.Log("Undo function called");
        if (undoActions.Count > 0)
        {
            UserAction prevAction = undoActions.Pop();
            int nextState = ButtonScript.instance.puzzle.GridData[prevAction.savedRow, prevAction.savedCol];
            redoActions.Push(new UserAction(prevAction.savedRow, prevAction.savedCol, nextState));

            ButtonScript.instance.puzzle.GridData[prevAction.savedRow, prevAction.savedCol] = prevAction.savedState;
            ButtonScript.instance.UndoCell(); // Modify cell State in ButtonScript
        }
        else
        {
            Debug.Log("Undo count has run out");
        }
    }

    public void Redo()
    {
        Debug.Log("Redo function called");
        if (redoActions.Count > 0)
        {
            UserAction nextAction = redoActions.Pop();
            int nextState = ButtonScript.instance.puzzle.GridData[nextAction.savedRow, nextAction.savedCol];
            undoActions.Push(new UserAction(nextAction.savedRow, nextAction.savedCol, nextState));

            ButtonScript.instance.puzzle.GridData[nextAction.savedRow, nextAction.savedCol] = nextAction.savedState;
            ButtonScript.instance.RedoCell(); // Modify cell State in ButtonScript
        }
        else
        {
            Debug.Log("Redo count has run out");
        }
    }

    // Detects Ctrl Z and Ctrl Y for Undo and Redo
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Ctrl Z kept undoing my asset changes, so I changed it to Ctrl E for now lol
            if (Input.GetKeyDown(KeyCode.E))
            {
                Undo();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Redo();
            }
        }
    }

    public struct UserAction
    {
        public int savedRow, savedCol, savedState;
        public UserAction(int saveRow, int saveCol, int saveState)
        {
            savedRow = saveRow;
            savedCol = saveCol;
            savedState = saveState;
        }
    }
}
