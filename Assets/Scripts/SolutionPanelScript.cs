using UnityEngine;
using UnityEngine.UI;

public class SolutionPanelScript : MonoBehaviour
{
    public void SetSolutionScreen(Sprite solutionSprite)
    {
        GetComponentInChildren<Image>().sprite = solutionSprite;
    }
    
}
