using System.Collections;
using UnityEngine;

public class BoardClicker : MonoBehaviour
{
    Gem clickedGem;


    // Update is called once per frame
    void Update()
    {
        // do not allow clicking when game finished
        if (BoardChecker.Instance.boardFinished)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            clickedGem = GetGem();
            if (clickedGem == null)
                return;
        }        

        if (clickedGem != null && Input.GetKeyUp(KeyCode.Mouse0))
        {
            Gem gemToSwap = GetGem();
            if(gemToSwap != null)
            {
                StartCoroutine(SwapGems(clickedGem, gemToSwap));
            }
            clickedGem = null;
        }
    }


    Gem GetGem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask(new string[] { "Gem" });

        if(Physics.Raycast(ray,out var hit,100, layerMask))
        {
            Gem clickedGem = hit.transform.GetComponent<Gem>();
            if (clickedGem == null)
                return null;

            if (!clickedGem.Stationed())
                return null;

            return clickedGem;
        }

        return null;
    }

    IEnumerator SwapGems(Gem a, Gem b)
    {
        float startTime = Time.time;
        float duration = 0.5f;

        Vertex a_vertex = a.vertex;
        Vertex b_vertex = b.vertex;

        a.vertex.reserved = true;
        b.vertex.reserved = true;


        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;

            a.transform.position = Vector3.Lerp(
                a_vertex.transform.position,
                b_vertex.transform.position,
                t
                );

            b.transform.position = Vector3.Lerp(
                b_vertex.transform.position,
                a_vertex.transform.position,
                t
                );
            yield return null;
        }

        
        a_vertex.PassGemToOtherHolder(b_vertex);
        a_vertex.ReceiveGem(b);
    }
}
