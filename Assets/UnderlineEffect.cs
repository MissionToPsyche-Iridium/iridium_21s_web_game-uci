using UnityEngine;
using UnityEngine.UI;

public class UnderlineEffect : MonoBehaviour
{
    public RawImage underline;
    public float speed = 0.2f;

    public void ShowUnderline()
    {
        underline.rectTransform.localScale = new Vector3(0, 1, 1);
        underline.rectTransform.LeanScaleX(1f, speed).setEaseOutExpo();
    }

    public void HideUnderline()
    {
        underline.rectTransform.LeanScaleX(0f, speed).setEaseOutExpo();
    }
}
