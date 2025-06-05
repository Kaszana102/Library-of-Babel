using UnityEngine;

public class ChapterSelector : MonoBehaviour
{

    public static ChapterSelector Instance;

    public int selectedChapter;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void GoToChapter(int index)
    {
        selectedChapter = index;
        PlayerWalker.Instance.GoToChapter(
            ChapterPoints.Instance.chapterPoints[index]
            );
    }
}
