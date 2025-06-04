using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vertex))]
public class VertexEditor : Editor
{
    bool connecting_flow = false;
    bool connecting_connection = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (connecting_flow)
        {
            if (GUILayout.Button("Stop connecting"))
            {
                connecting_flow = false;
            }
        }
        else
        {
            if (GUILayout.Button("Connect flow"))
            {
                connecting_flow = true;
                connecting_connection = false;
            }
        }


        if (connecting_connection)
        {
            if (GUILayout.Button("Stop connecting_connection"))
            {
                connecting_connection = false;
            }
        }
        else
        {
            if (GUILayout.Button("Connect connection"))
            {
                connecting_connection = true;
                connecting_flow = false;
            }
        }
    }



    private void OnSceneGUI()
    {
        if (connecting_flow || connecting_connection)
        {
            Event e = Event.current;

            //Check the event type and make sure it's left click.
            if ((e.type == EventType.MouseDown) && e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                ray.direction = new Vector3(0, 0, 1);
                object rayCastResult = (RaycastHit)HandleUtility.RaySnap(ray);
                if (rayCastResult != null)
                {
                    RaycastHit hit = (RaycastHit)rayCastResult;
                    Transform parent = hit.transform.parent;
                    Vertex vertex = parent.GetComponent<Vertex>();
                    if (vertex != null)
                    {
                        ConnectVertex(vertex);
                        Debug.Log("CLICKED");
                    }
                }

                e.Use();  //Eat the event so it doesn't propagate through the editor.
                connecting_flow = false;
                connecting_connection = false;
            }
        }
    }

    void ConnectVertex(Vertex end)
    {
        if (connecting_connection)
        {
            ConnectConnectionVertex(end);
        }
        else if (connecting_flow)
        {
            ConnectFlowVertex(end);
        }
    }

    void ConnectFlowVertex(Vertex end)
    {
        Vertex vertex = target as Vertex;
        GameObject edgesRoot = GetEdgesRoot();
        Edge edgePrefab = Resources.Load<Edge>("Prefabs/Board/Edge");
        Edge edge = Instantiate(edgePrefab);
        edge.transform.SetParent(edgesRoot.transform);
        edge.start = vertex;
        edge.end = end;

        vertex.outflow.Add(edge);
        end.inflow.Add(edge);

        PrefabUtility.RecordPrefabInstancePropertyModifications(vertex);
        PrefabUtility.RecordPrefabInstancePropertyModifications(end);
    }

    void ConnectConnectionVertex(Vertex end)
    {
        Vertex vertex = target as Vertex;
        GameObject edgesRoot = GetEdgesRoot();
        Edge edgePrefab = Resources.Load<Edge>("Prefabs/Board/Edge");
        Edge edge = Instantiate(edgePrefab);
        edge.transform.SetParent(edgesRoot.transform);
        edge.start = vertex;
        edge.end = end;

        vertex.connection.Add(edge);
        end.connection.Add(edge);

        PrefabUtility.RecordPrefabInstancePropertyModifications(vertex);
        PrefabUtility.RecordPrefabInstancePropertyModifications(end);
    }
    GameObject GetEdgesRoot()
    {
        GameObject edgesRoot = GameObject.FindGameObjectWithTag("EdgesRoot");
        if (edgesRoot == null)
        {
            edgesRoot = new GameObject("Edges Root");
            edgesRoot.tag = "EdgesRoot";
        }
        return edgesRoot;
    }
}
