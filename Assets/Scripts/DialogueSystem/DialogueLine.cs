using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private Text textHolder;

        [Header("Text Options")]
        [SerializeField] private string input;
        [SerializeField] private Font textFont;
        [SerializeField] private Color textColor;

        [Header("Time Parameters")]
        [SerializeField] private float delay;

        [Header("Character Image")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;

        private void Awake()
        {
            textHolder = GetComponent<Text>();
            textHolder.text = "";

            StartCoroutine(WriteText(input, textHolder, textColor, textFont, delay));
            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }
    }
}

