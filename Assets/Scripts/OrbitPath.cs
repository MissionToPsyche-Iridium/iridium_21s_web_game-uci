using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpiralOrbit : MonoBehaviour
{
    public int points = 500;           // Higher number of points for smoothness
    public float turns = 3f;         
    public float maxRadius = 7f;      // Overall size of the spiral
    public float dashRepeatRate = 20f; // Controls dash tiling
    public Material spiralMaterial;   

    void Start()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.material = spiralMaterial;
        line.widthMultiplier = 0.15f;            
        line.textureMode = LineTextureMode.Tile; 
        line.positionCount = points;

        float angleIncrement = (360f * turns) / points;

        for (int i = 0; i < points; i++)
        {
            float angle = Mathf.Deg2Rad * angleIncrement * i;
            float radius = Mathf.Lerp(0, maxRadius, i / (float)(points - 1));

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0));
        }

        line.material.mainTextureScale = new Vector2(dashRepeatRate, 1f);
    }
}
