using UnityEngine;
using TMPro;

public class ShowLevelsHover : MonoBehaviour
{
    public TextMeshProUGUI[] planetLabels;

    void Start()
    {
        foreach (var label in planetLabels)
            label.enabled = false;
    }

    void OnMouseEnter()
    {
        Debug.Log("Hover detected on Show Levels");  
        foreach (var label in planetLabels)
            label.enabled = true; 
    }

    void OnMouseExit()
    {
        foreach (var label in planetLabels)
            label.enabled = false; 
    }
}
