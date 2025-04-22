using UnityEngine;
using TMPro;

public class PlanetHoverZoom : MonoBehaviour
{
    public float hoverScale = 1.3f;
    public float scaleSpeed = 10f;
    public TextMeshProUGUI levelLabel;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (levelLabel != null)
        {
            levelLabel.gameObject.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * hoverScale));

        if (!ShowLevelsHover.showAllActive && levelLabel != null)
        {
            levelLabel.gameObject.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));

        if (!ShowLevelsHover.showAllActive && levelLabel != null)
        {
            levelLabel.gameObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator ScaleTo(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
