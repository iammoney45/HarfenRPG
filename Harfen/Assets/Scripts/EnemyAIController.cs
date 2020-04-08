using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public float walkRadius;

    public NavMeshAgent agent;

    private bool reachedDestination = true;

    // Start is called before the first frame update
    void Start()
    {
        //agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(reachedDestination)
        {
            agent.SetDestination(updatePosition());
            reachedDestination = false;
        }
        else
        {
            if(ArrivedAtDestination()) { reachedDestination = true; }
        }
    }

    public Vector3 updatePosition()
    {
        print("Setting positon");
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }

    public bool ArrivedAtDestination()
    {
        if(agent.remainingDistance < 5f) { return true; }
        else { return false; }
    }
}
