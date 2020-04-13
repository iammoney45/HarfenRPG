using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    public GameObject player;

    //for enemy AI
    public Vector3 target;
    public float walkRadius;

    private NavMeshAgent agent;
    private bool reachedDestination = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        target = hit.position;
        this.GetComponent<Animator>().SetBool("Walking", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (reachedDestination)
        {
            updatePosition();
            reachedDestination = false;
        }
        else
        {
            if (ArrivedAtDestination()) { reachedDestination = true; }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("hit by: " + collision.gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            Kill();
        }

    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void updatePosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        target = hit.position;
        agent.SetDestination(target);
        print("new position is: " + target.x + " " + target.y + " " + target.z);
    }

    public bool ArrivedAtDestination()
    {
        if (agent.remainingDistance < 5f) { return true; }
        else { return false; }
    }
}
