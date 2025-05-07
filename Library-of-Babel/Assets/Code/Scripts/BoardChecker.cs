using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardChecker : MonoBehaviour
{
    public static BoardChecker Instance;

    public List<Gem> gems = new List<Gem>();   


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        GemCreator.SelectNewGems();
    }



    // Update is called once per frame
    void Update()
    {
        List<Gem> gemsToDelete = new List<Gem>();
        for(int i = gems.Count -1; i>=0; i--)
        {
            Gem gem = gems[i];
            if (!gem.Stationed())
                continue;
            if (GemInSequence(gem, out var gemsToDrop))
            {
                foreach(Gem gemToDrop in gemsToDrop)
                {
                    gemToDrop.Drop();
                }
                gemsToDelete.AddRange(gemsToDrop);
            }
        }
        if(gemsToDelete.Count > 0)
        {
            gems.RemoveAll(x => gemsToDelete.Contains(x)); ;
            gemsToDelete.Clear();
        }
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
}
