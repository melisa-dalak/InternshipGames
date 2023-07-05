using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wall : MonoBehaviour
{
    public Transform point1;
    public Transform point2;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        if (point1 != null && point2 != null)
        {
            lineRenderer.SetPosition(0, point1.position);
            lineRenderer.SetPosition(1, point2.position);
        }
    }
}
