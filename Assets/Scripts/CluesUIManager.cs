using UnityEngine;
using TMPro;

public class CluesUIManager : MonoBehaviour
{
    public static CluesUIManager instance;

    [SerializeField] Transform rowCluesParent, colCluesParent;

    private void Awake()
    {
        instance = this;
    }
    public void UpdateClues(NonogramPuzzle puzzle)
    {
        //Update rows
        for (int r = 0; r < puzzle.RowClues.Length; r++)
        {
            string clueText = string.Join(" ", puzzle.RowClues[r].Clues);
            rowCluesParent.GetChild(r).GetComponent<TMP_Text>().text = clueText;
        }
        //Update columns
        for (int c = 0; c < puzzle.ColClues.Length; c++)
        {
            string clueText = string.Join("\n", puzzle.ColClues[c].Clues);
            colCluesParent.GetChild(c).GetComponent<TMP_Text>().text = clueText;
        }
    }
}
