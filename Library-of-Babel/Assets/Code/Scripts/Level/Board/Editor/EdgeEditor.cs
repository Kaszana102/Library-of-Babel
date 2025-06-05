using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Edge))]
[CanEditMultipleObjects]
public class EdgeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Delete edge"))
        {
            Edge edge = (Edge)target;

            Undo.RecordObject(edge.start, "remove Flow Start");
            Undo.RecordObject(edge.end, "Remove Flow End");


            edge.start.inflow.Remove(edge);
            edge.start.outflow.Remove(edge);
            edge.start.connection.Remove(edge);

            edge.end.inflow.Remove(edge);
            edge.end.outflow.Remove(edge);
            edge.end.connection.Remove(edge);


            Undo.DestroyObjectImmediate(edge.gameObject);
        }
        
    }
}
