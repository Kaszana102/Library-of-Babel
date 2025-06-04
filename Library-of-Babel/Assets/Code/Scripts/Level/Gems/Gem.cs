using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public enum GemType
    {
        BLACK,
        RED,
        BLUE,
        GREEN,
        YELLOW,
        PURPLE
    }

    public GemType type = GemType.BLACK;
    [HideInInspector]
    public Vertex vertex;


    public bool Stationed()
    {
        return vertex != null;
    }

    public void Drop()
    {
        vertex.gem = null;
        StartCoroutine(Dropping());
    }

    IEnumerator Dropping()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(new Vector3(1, 0, 0));
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }   
}
