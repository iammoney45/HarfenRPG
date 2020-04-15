﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    public GameObject player;

    //for enemy AI
    public Vector3 target;
    public float walkRadius;
    public float deathAnimTime;
    public float attackDistance;
    public float swordAnimTime;
    public float magicAnimTime;
    public float clearAnimTime;

    private NavMeshAgent agent;
    private bool reachedDestination = false;
    private IEnumerator waitForDeathCoroutine;
    private IEnumerator switchToSwordCoroutine;
    private IEnumerator switchToMagicCoroutine;
    private IEnumerator clearInventoryCoroutine;
    private bool stopMoving = false;
    private bool attackingPlayer = false;
    private EnemyInventoryScript enemyInventoryScript;
    private float switchCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyInventoryScript = GetComponent<EnemyInventoryScript>();
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        target = hit.position;
        agent.SetDestination(target);
        this.GetComponent<Animator>().SetBool("Walking", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerAttacking())
        {
            attackingPlayer = true;
            agent.SetDestination(player.transform.position);
            if (!this.GetComponent<Animator>().GetBool("HasSword"))
            {
                switchToSwordCoroutine = SwitchToSword(swordAnimTime);
                StartCoroutine(switchToSwordCoroutine);
            }
        }
        else
        {
            attackingPlayer = false;
            if(this.GetComponent<Animator>().GetBool("HasSword"))
            {
                clearInventoryCoroutine = ClearInventory(clearAnimTime);
                StartCoroutine(clearInventoryCoroutine);
            }
            else if (reachedDestination && !stopMoving && !attackingPlayer)
            {
                updatePosition();
                reachedDestination = false;
            }
            else
            {
                if (ArrivedAtDestination()) { reachedDestination = true; }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("hit by: " + collision.gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hit by: " + other.gameObject.tag);
        if (player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordAttack"))
        {
            Kill();
        }

    }

    public void Kill()
    {
        waitForDeathCoroutine = WaitForDeathAnim(deathAnimTime);
        StartCoroutine(waitForDeathCoroutine);
    }

    public void updatePosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        target = hit.position;
        agent.SetDestination(target);
    }

    public bool ArrivedAtDestination()
    {
        if (agent.remainingDistance < 5f) { return true; }
        else { return false; }
    }

    private bool PlayerAttacking()
    {
        if((player.GetComponent<Animator>().GetBool("HasSword") || player.GetComponent<Animator>().GetBool("HasMagic"))
            && Mathf.Abs((this.transform.position - player.transform.position).magnitude) < attackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator WaitForDeathAnim(float waitTime)
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
        agent.SetDestination(transform.position);
        stopMoving = true;
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    private IEnumerator SwitchToSword(float waitTime)
    {
        this.GetComponent<Animator>().SetBool("Walking", false);
        agent.SetDestination(transform.position);
        enemyInventoryScript.EquipSword();
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<Animator>().SetBool("Walking", true);
        agent.SetDestination(player.transform.position);
    }

    private IEnumerator SwitchToMagic(float waitTime)
    {
        this.GetComponent<Animator>().SetBool("Walking", false);
        agent.SetDestination(transform.position);
        enemyInventoryScript.EquipMagic();
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<Animator>().SetBool("Walking", true);
        agent.SetDestination(player.transform.position);
    }

    private IEnumerator ClearInventory(float waitTime)
    {
        this.GetComponent<Animator>().SetBool("Walking", false);
        agent.SetDestination(transform.position);
        enemyInventoryScript.ClearInventory();
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<Animator>().SetBool("Walking", true);
        updatePosition();
        reachedDestination = false;
    }
}
