using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D satelliteCursor;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        SetSatelliteCursor();
    }

    public void SetSatelliteCursor()
    {
        if (satelliteCursor != null)
        {
            Vector2 hotspot = new Vector2(satelliteCursor.width / 2, satelliteCursor.height / 2);
            Cursor.SetCursor(satelliteCursor, hotspot, cursorMode);
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);  
    }
}
