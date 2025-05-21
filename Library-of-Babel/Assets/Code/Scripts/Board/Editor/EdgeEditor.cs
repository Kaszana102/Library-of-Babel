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

            edge.start.inflow.Remove(edge);
            edge.start.outflow.Remove(edge);
            edge.start.connection.Remove(edge);

            edge.end.inflow.Remove(edge);
            edge.end.outflow.Remove(edge);
            edge.end.connection.Remove(edge);


            PrefabUtility.RecordPrefabInstancePropertyModifications(edge);
            PrefabUtility.RecordPrefabInstancePropertyModifications(edge.start);
            PrefabUtility.RecordPrefabInstancePropertyModifications(edge.end);
            DestroyImmediate(edge.gameObject);
        }
        
    }
}
