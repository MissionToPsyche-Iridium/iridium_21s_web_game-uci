using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUndoRedo : MonoBehaviour
{
    public static UIUndoRedo instance;
    public Stack<GameObject> undoActions = new Stack<GameObject>();
    public Stack<GameObject> redoActions = new Stack<GameObject>();
    AudioManager sounds;

    private void Awake()
    {
        instance = this;
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Undo()
    {
        Debug.Log("Undo function called");
        if (undoActions.Count > 0)
        {
            sounds.PlaySFX(sounds.undoRedoSFX);

            redoActions.Push(undoActions.Peek());
            undoActions.Peek().GetComponent<ButtonScript>().UndoCell();
            undoActions.Pop();
        }
        else
        {
            sounds.PlaySFX(sounds.emptyInput);
            Debug.Log("Nothing left to undo");
        }
    }

    public void Redo()
    {
        Debug.Log("Redo function called");
        if (redoActions.Count > 0)
        {
            sounds.PlaySFX(sounds.undoRedoSFX);
            undoActions.Push(redoActions.Peek());
            redoActions.Peek().GetComponent<ButtonScript>().RedoCell();
            redoActions.Pop();
        }
        else
        {
            sounds.PlaySFX(sounds.emptyInput);
            Debug.Log("Nothing left to redo");
        }
    }

    // Detects Ctrl Z and Ctrl Y for Undo and Redo
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Undo();
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Redo();
            }
        }
    }
}
