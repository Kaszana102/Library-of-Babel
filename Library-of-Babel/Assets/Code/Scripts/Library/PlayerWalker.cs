using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class PlayerWalker : MonoBehaviour
{
    public static PlayerWalker Instance;

    NavMeshAgent agent;
    bool walking = false;
    bool rotating = false;

    GameObject destinationChapter;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (walking)
        {
            if (FinishedWalking())
                OnWalkFinished();                

            return;
        }

        if (rotating)
            return;

    }

    #region walking
    public void GoToChapter(GameObject destination)
    {
        walking = true;
        destinationChapter = destination;
        agent.SetDestination(destinationChapter.transform.position);
    }


    bool FinishedWalking()
    {
        return !agent.pathPending && !agent.hasPath;
    }

    void OnWalkFinished()
    {
        walking = false;
        rotating = true;
        StartCoroutine(RotatePlayer());

        Debug.Log("DISPLAY CHAPTER");
    }

    IEnumerator RotatePlayer()
    {       
        Quaternion startRot = transform.rotation;
        Quaternion endRot = destinationChapter.transform.rotation;
        float rotationTime = .2f;
        float startTime = Time.time;

        while(Time.time < startTime + rotationTime)
        {
            float t = (Time.time - startTime) / rotationTime;
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        transform.rotation = endRot;
        rotating = false;
    }

    #endregion

}
