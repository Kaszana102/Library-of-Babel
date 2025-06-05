using System.Collections.Generic;
using UnityEngine;

public class ChapterPoints : MonoBehaviour
{
    public static ChapterPoints Instance;

    public List<GameObject> chapterPoints = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
}
