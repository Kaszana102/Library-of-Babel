using UnityEngine;

public class Edge : GemHolder
{
    [SerializeField] public Vertex start;
    [SerializeField] public Vertex end;

    float startTime;
    float maxTime = 1f;

    LineRenderer lineRenderer;
    uint hp = 1;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        UpdateColor();
    }

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

    public bool HasVertex(Vertex vertex)
    {
        return start == vertex || end == vertex;
    }

    public void Decrease()
    {
        if(hp == 0)    
            return;
        hp--;
        UpdateColor();
    }

    void UpdateColor()
    {
        switch (hp)
        {
            case 2:
                SetColor(Color.black);
                break;
            case 1:
                SetColor(Color.gray);
                break;
            case 0:
                SetColor(Color.white);
                break;
            default:
                SetColor(Color.magenta);
                break;
        }   
    }

    void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public bool Finished()
    {
        return hp == 0;
    }
}
