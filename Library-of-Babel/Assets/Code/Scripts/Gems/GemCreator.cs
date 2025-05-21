using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GemCreator
{

    static List<Gem> allGemPrefabs = Resources.LoadAll<Gem>("Prefabs/Gems/FinalGems/").ToList();
    static List<Gem> selectedPrefabs;

    public static void SelectNewGems()
    {     
        selectedPrefabs = allGemPrefabs;
        selectedPrefabs.RemoveRange(2, 1);
    }

    public static Gem CreateGem()
    {
        int index = Random.Range(0, selectedPrefabs.Count);

        Gem gemGameObject = GameObject.Instantiate<Gem>(selectedPrefabs[index]);
        BoardChecker.Instance.gems.Add(gemGameObject);

        return gemGameObject;
    }
}
