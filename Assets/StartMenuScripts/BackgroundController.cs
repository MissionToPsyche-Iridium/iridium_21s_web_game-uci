using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Unity.VisualScripting;

public class BackgroundController : MonoBehaviour
{
    public GameObject puzzle;
    public PuzzleManager puzzleManager;
    public TextMeshProUGUI gameTitle;
    public CanvasGroup[] menuButtons;
    public CanvasGroup arrowIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
    }

    IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(1.5f);

        puzzle.SetActive(true);
        yield return StartCoroutine(puzzleManager.StartPuzzleAnimation());

        gameTitle.gameObject.SetActive(true);
        LeanTween.alphaText(gameTitle.rectTransform, 1f, 0.7f).setEase(LeanTweenType.easeInOutSine);

        yield return new WaitForSeconds(1f);

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
    // Update is called once per frame
    void Update()
    {

    }
}
