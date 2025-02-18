using UnityEngine;

public class UIScalerScript : MonoBehaviour
{

    [SerializeField] RectTransform puzzleContainer;
    [SerializeField] float zoomSpeed = 0.1f, minScale = 0.5f, maxScale = 3;
    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0)
        {
            float scaleChange = scroll * zoomSpeed;
            Vector3 newScale = puzzleContainer.localScale + new Vector3(scaleChange, scaleChange, scaleChange);
            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale, maxScale),
                Mathf.Clamp(newScale.y, minScale, maxScale),
                1
                );
            puzzleContainer.localScale = newScale;
        }
    }
}
