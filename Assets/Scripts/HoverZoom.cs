using UnityEngine;

public class PlanetHoverZoom : MonoBehaviour
{
    public float hoverScale = 1.3f;  
    public float scaleSpeed = 10f; 

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; 
    }

    void OnMouseEnter()
    {
        StopAllCoroutines(); 
        StartCoroutine(ScaleTo(originalScale * hoverScale));
    }

    void OnMouseExit()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
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
