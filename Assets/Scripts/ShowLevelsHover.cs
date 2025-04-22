using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowLevelsHover : MonoBehaviour
{
    public List<TextMeshProUGUI> levelLabels;
    public static bool showAllActive = false;  // GLOBAL toggle flag

    public void ShowPlanetLabels()
    {
        foreach (var label in levelLabels)
        {
            label.gameObject.SetActive(true);
        }

        showAllActive = true;
    }

    public void HidePlanetLabels()
    {
        foreach (var label in levelLabels)
        {
            label.gameObject.SetActive(false);
        }

        showAllActive = false;
    }
}
