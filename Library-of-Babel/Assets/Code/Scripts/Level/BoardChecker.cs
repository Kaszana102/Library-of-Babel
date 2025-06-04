using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardChecker : MonoBehaviour
{
    public static BoardChecker Instance;

    public List<Gem> gems = new List<Gem>();

    public bool boardFinished { private set; get; } = false;
    List<Edge> edges;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        GemCreator.SelectNewGems();
        edges = FindObjectsByType<Edge>(FindObjectsSortMode.None).ToList();
    }



    // Update is called once per frame
    void Update()
    {
        if (!boardFinished)
        {
            List<Gem> gemsToDelete = new List<Gem>();
            for (int i = gems.Count - 1; i >= 0; i--)
            {
                Gem gem = gems[i];
                if (!gem.Stationed())
                    continue;
                if (GemInSequence(gem, out var gemsToDrop))
                {
                    var edges = getConnectedEdges(gemsToDrop);
                    foreach (Edge edge in edges)
                    {
                        edge.Decrease();
                    }


                    foreach (Gem gemToDrop in gemsToDrop)
                    {
                        gemToDrop.Drop();
                    }
                    gemsToDelete.AddRange(gemsToDrop);
                }
            }
            if (gemsToDelete.Count > 0)
            {
                gems.RemoveAll(x => gemsToDelete.Contains(x)); ;
                gemsToDelete.Clear();
            }


            CheckEdgesCompletion();
        }
        
    }

    void CheckEdgesCompletion()
    {
        bool allEdgesCompleted = true;
        foreach(Edge edge in edges)
        {
            if (!edge.Finished())
            {
                allEdgesCompleted = false;
                break;
            }
        }
        if (allEdgesCompleted)
        {
            Debug.Log("GAME FINSIHED");
            boardFinished = true;
        }
    }

    List<Edge> getConnectedEdges(List<Gem> gemGroup)
    {
        List<Edge> edges = new List<Edge>();
        
        foreach(Gem gem in gemGroup)
        {
            foreach(Edge edge in gem.vertex.edges)
            {
                Vertex otherEnd = edge.GetOtherEnd(gem.vertex);
                Gem otherGem = otherEnd.gem;
                if (otherGem == null)
                    continue;

                if (!gemGroup.Contains(otherGem))
                    continue;

                if (edges.Contains(edge))
                    continue;
                edges.Add(edge);
            }
        }
        return edges;
    }

    bool GemInSequence(Gem startGem, out List<Gem> gemsToDrop)
    {
        List<Gem> gemsToCheck = new List<Gem>();
        List<Gem> checkedGems = new List<Gem>() { startGem };
        gemsToDrop = new List<Gem>() { startGem };

        gemsToCheck = startGem.vertex.NeighboursGems();
        Gem gem;
        while (gemsToCheck.Count > 0)
        {
            // get neighbour
            gem = gemsToCheck[0];
            gemsToCheck.RemoveAt(0);
            if (gem == null)
                continue;

            if (checkedGems.Contains(gem))
                continue;
            checkedGems.Add(gem);

            if (gem.Stationed() && gem.type == startGem.type)
            {
                gemsToDrop.Add(gem);
                gemsToCheck.AddRange(gem.vertex.NeighboursGems());
            }
        }

        int foundGems = gemsToDrop.Count;
        if (foundGems < 3)
        {
            gemsToDrop.Clear();
            gemsToDrop = null;
        }

        return foundGems >= 3;
    }

    // Check from the start vertex, where the given gem will be moved to, if the 
    // gem will be in group
    public bool WillBeInGroup(Vertex start, Gem gemToCheck)
    {
        List<Gem> gemsToCheck = new List<Gem>();
        List<Gem> checkedGems = new List<Gem>() { gemToCheck };
        List<Gem> gemsInGroup = new List<Gem>() { gemToCheck};

        gemsToCheck = start.NeighboursGems();
        Gem gem;
        while (gemsToCheck.Count > 0)
        {
            // get neighbour
            gem = gemsToCheck[0];
            gemsToCheck.RemoveAt(0);
            if (gem == null)
                continue;

            if (checkedGems.Contains(gem))
                continue;
            checkedGems.Add(gem);

            if (gem.Stationed() && gem.type == gemToCheck.type)
            {
                gemsInGroup.Add(gem);
                gemsToCheck.AddRange(gem.vertex.NeighboursGems());
            }
        }

        int foundGems = gemsInGroup.Count;
        return foundGems >= 3;
    }
}
