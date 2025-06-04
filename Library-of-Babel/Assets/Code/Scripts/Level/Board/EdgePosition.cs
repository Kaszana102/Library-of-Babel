using UnityEngine;

[ExecuteInEditMode]
public class EdgePosition : MonoBehaviour
{
    Edge edge;
    LineRenderer lineRenderer;

    private void Start()
    {
        edge = GetComponent<Edge>();
        
    }

    private void Update()
    {
        if(lineRenderer == null){
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (edge.end == null || edge.start == null)
            return;

        edge.transform.position = (edge.start.transform.position + edge.end.transform.position)/2;

        var start_offset = edge.start.transform.position;
        var end_offset = edge.end.transform.position;

        lineRenderer.SetPosition(0, start_offset);
        lineRenderer.SetPosition(1, end_offset);
    }
}
