using UnityEngine;

public class Edge : GemHolder
{
    [SerializeField]public Vertex start;
    [SerializeField] public Vertex end;

    float startTime;
    float maxTime = 1f;

    protected override void OnPassGem()
    {
        gem.vertex = null;
        gem.transform.position = start.transform.position;
        startTime = Time.time;
        end.SetIncomingGem();
    }


    // Update is called once per frame
    void Update()
    {
        if (!Occupied())
            return;

        float percentage = (Time.time - startTime) / maxTime;
        if (percentage >= 1.0)
        {
            PassGemToOtherHolder(end);
        }
        else
        {
            PositionGem(percentage);
        }
    }

    void PositionGem(float percentage)
    {
        gem.transform.position = Vector3.Lerp(
            start.transform.position,
            end.transform.position,
            percentage
            );
    }

    public Vertex GetOtherEnd(Vertex oneEnd)
    {
        if (start == oneEnd)
        {
            return end;
        }
        return start;
    }
}
