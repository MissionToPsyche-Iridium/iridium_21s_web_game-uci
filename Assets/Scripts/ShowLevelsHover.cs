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

    public void ShowPlanetLabels()
    {
        foreach (var label in planetLabels)
            label.enabled = true;
    }

    public void HidePlanetLabels()
    {
        foreach (var label in planetLabels)
            label.enabled = false; 
    }
}
