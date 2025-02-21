using UnityEngine;

public class OrbitMover : MonoBehaviour
{
    public SpiralOrbit spiralOrbit;
    public float speed = 1f;  // Movement speed along the spiral

    private float t = 0f;

    void Update()
    {
        if (spiralOrbit == null) return;

        t += speed * Time.deltaTime;
        t = Mathf.Repeat(t, 1f); // Loop motion

        int index = Mathf.FloorToInt(t * (spiralOrbit.points - 1));
        transform.position = spiralOrbit.GetComponent<LineRenderer>().GetPosition(index);
    }
}
