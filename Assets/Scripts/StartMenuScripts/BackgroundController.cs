using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

using System;
using Unity.VisualScripting;

public class BackgroundController : MonoBehaviour
{
    public GameObject puzzle;
    public PuzzleManager puzzleManager;
    public TextMeshProUGUI gameTitle;
    public CanvasGroup[] menuButtons;
    public static bool hasPlayed = false;
    [SerializeField] CanvasGroup arrowIndicator, optionPanel, menuOptions;
    AudioManager sounds;

    private void Awake()
    {
        sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        optionPanel.alpha = 0f;
        optionPanel.interactable = false;
        optionPanel.blocksRaycasts = false;
        if (hasPlayed)
        { 
            gameTitle.gameObject.SetActive(true);
            StartCoroutine(ShowButtons());

        }
        else
        {
            puzzle.SetActive(false);
            gameTitle.gameObject.SetActive(false);
            
            arrowIndicator.alpha = 0f;
            foreach (CanvasGroup button in menuButtons)
            {
                button.alpha = 0f;
                button.interactable = false;
                button.blocksRaycasts = false;
            }

            StartCoroutine(ShowMenu());
            hasPlayed = true;
        }

    }

    IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(1.5f);

        puzzle.SetActive(true);
        yield return StartCoroutine(puzzleManager.StartPuzzleAnimation());

        gameTitle.gameObject.SetActive(true);
        LeanTween.alphaText(gameTitle.rectTransform, 1f, 0.7f).setEase(LeanTweenType.easeInOutSine);

        yield return new WaitForSeconds(1f);

        StartCoroutine(ShowButtons());

    }

    IEnumerator ShowButtons()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (i == 0)
            {
                LeanTween.alphaCanvas(arrowIndicator, 1f, 0.7f).setEase(LeanTweenType.easeInOutSine);
            }
            LeanTween.alphaCanvas(menuButtons[i], 1f, 0.7f).setEase(LeanTweenType.easeInOutSine).setDelay(i * 0.1f);
            yield return new WaitForSeconds(0.3f);
            menuButtons[i].interactable = true;
            menuButtons[i].blocksRaycasts = true;
        }
    }

    public void ToggleOptionsPanel()
    {
        sounds.PlaySFX(sounds.generalUIButton);
        if (optionPanel.alpha == 0f)
        {
            optionPanel.alpha = 1f;
            optionPanel.interactable = true;
            optionPanel.blocksRaycasts = true;
            menuOptions.alpha = 0f;
            menuOptions.interactable = false;
            menuOptions.blocksRaycasts = false;
        }
        else
        {
            optionPanel.alpha = 0f;
            optionPanel.interactable = false;
            optionPanel.blocksRaycasts = false;
            menuOptions.alpha = 1f;
            menuOptions.interactable = true;
            menuOptions.blocksRaycasts = true;
        }
    }

    public void ToggleCreditsScene()
    {
        sounds.PlaySFX(sounds.generalUIButton);
        SceneManager.LoadScene("CreditsScene");
    }

}
