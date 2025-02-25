using UnityEngine;

public class UIScalerScript : MonoBehaviour
{

    [SerializeField] RectTransform puzzleContainer;
    [SerializeField] float zoomSpeed = 0.1f, minScale = 0.5f, maxScale = 3;
    Vector2 originPosition = Vector2.zero;
    [SerializeField] float panSpeed = 1f;
    Vector3 lastMousePosition;
    bool isPanning = false;

    void Update()
    {
        ManageZoom();
        ManagePanning();
    }

    void ManageZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
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

    void ManagePanning()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isPanning = true;
            lastMousePosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(1) && isPanning)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            puzzleContainer.anchoredPosition += new Vector2(delta.x, delta.y) * panSpeed;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isPanning = false;
        }
    }

    public void ResetCameraPosition()
    {
        puzzleContainer.localScale = new Vector3(1, 1, 1);
        puzzleContainer.anchoredPosition = originPosition;
    }
}
