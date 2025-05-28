using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MapManager : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> levelSprite;
    [SerializeField] List<CanvasGroup> levelCanvasGroup;
    [SerializeField] GameObject outroDialogue;
    private int currentLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("MaxCurrentLevel", 0);

        SetupLevelNodes();

        //Added by Lance
        Debug.Log("Current Level: " + currentLevel);

        if (currentLevel < 3)
        {
            GameObject.Find("DialogueHolder End Game").SetActive(false);
        }
        else
        {
            StartCoroutine(WaitForOutroAndChangeScene());
        }

        if (currentLevel > 0)
        {
            GameObject.Find("DialogueHolder Intro").SetActive(false);
        }
    }

    private void SetupLevelNodes()
    {
        for (int i = 0; i < levelSprite.Count; i++)
        {
            bool isSpriteUnlocked = false;
            bool isCanvasUnlocked = false;

            if (i == currentLevel)
            {
                StartCoroutine(FadeInSprite(levelSprite[i], levelCanvasGroup[i], 30f));
                isCanvasUnlocked = true;
            }

            if (i < currentLevel)
            {
                isSpriteUnlocked = true;
                isCanvasUnlocked = true;
            }

            // Fade sprite
            Color spriteColor = levelSprite[i].color;
            spriteColor.a = isSpriteUnlocked ? 1f : 0f;
            levelSprite[i].color = spriteColor;

            // Deactivate interactable if the sprite is faded
            BoxCollider collider = levelSprite[i].GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = i <= currentLevel ? true : false;
            }

            // Fade canvas
            levelCanvasGroup[i].alpha = isCanvasUnlocked ? 1f : 0f;
            levelCanvasGroup[i].interactable = isCanvasUnlocked;
            levelCanvasGroup[i].blocksRaycasts = isCanvasUnlocked;
        }
    }

    IEnumerator FadeInSprite(SpriteRenderer sprite, CanvasGroup canvas, float duration)
    {
        float time = 0f;


        while (time < 1f)
        {
            time += Time.deltaTime / duration;
            Color spriteColor = sprite.color;
            spriteColor.a = Mathf.Lerp(sprite.color.a, 1f, time);
            sprite.color = spriteColor;
            yield return null;
        };

    }

    //Added by Lance
    private IEnumerator WaitForOutroAndChangeScene()
    {
        // Wait while the linkedDialogueHolder is active
        while (outroDialogue.activeSelf)
        {
            yield return null; // Wait for the next frame
        }

        // Once linkedDialogueHolder is inactive, change the scene
        SceneManager.LoadScene("CreditsScene");
    }

}
