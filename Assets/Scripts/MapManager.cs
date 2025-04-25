using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class MapManager : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> levelSprite;
    [SerializeField] List<CanvasGroup> levelCanvasGroup;
    private int currentLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("MaxCurrentLevel", 0);
        SetupLevelNodes();
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

}
