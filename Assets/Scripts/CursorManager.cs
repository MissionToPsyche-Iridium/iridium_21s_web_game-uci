using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D satelliteCursor;
    public Vector2 hotspot = Vector2.zero;  //anchor point
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        SetSatelliteCursor();
    }

    public void SetSatelliteCursor()
    {
        Cursor.SetCursor(satelliteCursor, hotspot, cursorMode);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);  
    }
}
