using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; private set; }
        AudioManager sounds;

        protected IEnumerator WriteText(string input, TextMeshProUGUI textHolder, Color textColor, TMP_FontAsset textFont, float delay, float delayBetweenLines)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;

            sounds = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            sounds.PlayDialogueSFX();

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                yield return new WaitForSeconds(delay);
            }

            sounds.StopDialogueSFX();

            yield return new WaitUntil(() => Input.GetMouseButton(0));

            finished = true;
        }
    }
}

