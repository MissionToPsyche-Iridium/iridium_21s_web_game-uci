using UnityEngine;

public class HelpMenuScript : MonoBehaviour
{
    [SerializeField] GameObject helpPanel;
    [SerializeField] CanvasGroup helpButton;

    private bool isHelpPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        helpPanel.SetActive(false);
    }

    public void ToggleHelpPanel()
    {
        // Toggle the help menu, pause the game and activate the help button
        isHelpPressed = !isHelpPressed;
        helpPanel.SetActive(isHelpPressed);

        if (isHelpPressed)
        {
            helpPanel.SetActive(true);
            helpButton.interactable = false;
            helpButton.alpha = 0f;
        }
        else
        {

            helpPanel.SetActive(false);
            helpButton.interactable = true;
            helpButton.alpha = 1f;
        }
    }
}
