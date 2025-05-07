using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Vertex : GemHolder
{
    [SerializeField] public List<Edge> inflow;
    [SerializeField] public List<Edge> outflow;
    [SerializeField] public List<Edge> connection;

    bool spawningGem = false;
    float spawnStart;
    float spawnDuration = 1f;

    bool hasIncomingGem = false;
    public List<Edge> edges {get; private set;}

    private void Start()
    {
        edges = new List<Edge>();
        edges.AddRange(inflow);
        edges.AddRange(outflow);
        edges.AddRange(connection);
           
    }

    // Update is called once per frame
    void Update()
    {
        if (!Occupied() && !hasIncomingGem)
        {
            if (CanSpawnGem())
            {
                SpawnGem();
            }
            return;
        }

        if (spawningGem)
        {
            float percentage = (Time.time - spawnStart) / spawnDuration;
            if (percentage >=1)
            {
                spawningGem = false;
                OnPassGem();
                gem.transform.localScale = Vector3.one;
                // allow moving the further if possible
            }
            else
            {
                gem.transform.position = Vector3.Lerp(
                    transform.position + Vector3.up,
                    transform.position,
                    percentage
                    ); ;
                gem.transform.localScale = Vector3.Lerp(
                    Vector3.zero,
                    Vector3.one,
                    percentage
                    ); ;
                return;
            }
        }

        if (!Occupied())
            return;

        // check if can pass gem to next vertex
        Edge edgeToPass = canPassFurther();
        if (edgeToPass == null){
            return;
        }

        PassGemToOtherHolder(edgeToPass);        

        
    }

    protected override void OnPassGem()
    {
        gem.transform.position = transform.position;
        gem.vertex = this;
        hasIncomingGem = false;
    }

    Edge canPassFurther()
    {
        foreach(Edge edge in outflow)
        {
            if (!edge.end.Occupied() && !edge.Occupied() && !edge.end.hasIncomingGem)
            {
                return edge;
            }
        }
        return null;
    }

    bool CanSpawnGem()
    {
        // check if node is viable for spawning
        if (inflow.Count != 0)
            return false;

        // check if not in progress of spawning
        if (spawningGem)
            return false;

        return true;
    }

    void SpawnGem()
    {
        gem = GemCreator.CreateGem();
        spawningGem = true;
        spawnStart = Time.time;
    }

    public List<Vertex> NeighbourVertices()
    {
        List<Vertex> neighbours = new List<Vertex>();
        foreach(Edge edge in edges)
        {
            neighbours.Add(edge.GetOtherEnd(this));
        }
        return neighbours;
    }

    public List<Gem> NeighboursGems()
    {
        List<Vertex> neighbours = new List<Vertex>();
        foreach (Edge edge in edges)
        {
            neighbours.Add(edge.GetOtherEnd(this));
        }
        return neighbours.Where(vertex => vertex.gem != null).Select(vertex => vertex.gem).ToList();
    }

    public void SetIncomingGem()
    {
        hasIncomingGem = true;
    }
}
