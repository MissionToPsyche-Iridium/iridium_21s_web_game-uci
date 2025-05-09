using UnityEngine;

[System.Serializable]
public struct HighlightData
{
    public Vector2 position;
    public Vector2 size;
}

public class TutorialHighlightController : MonoBehaviour
{
    [SerializeField] private RectTransform highlightBox;
    [SerializeField] private HighlightData[] highlights;

    public void ShowHighlightForStep(int step)
    {
        if (step < 0 || step >= highlights.Length)
        {
            highlightBox.gameObject.SetActive(false);
            return;
        }

        HighlightData data = highlights[step];
        highlightBox.anchoredPosition = data.position;
        highlightBox.sizeDelta = data.size;
        highlightBox.gameObject.SetActive(true);
    }

    public void HideHighlight()
    {
        highlightBox.gameObject.SetActive(false);
    }

    private void Start()
    {
        highlightBox.gameObject.SetActive(false);
    }
}
