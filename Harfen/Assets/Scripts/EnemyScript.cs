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
    public float deathAnimTime;
    public float drawWeaponDistance;
    public float attackDistance;
    public float swordAnimTime;
    public float magicAnimTime;
    public float clearAnimTime;
    public float swordSlashAnimTime;

    private NavMeshAgent agent;
    private bool reachedDestination = false;
    private IEnumerator waitForDeathCoroutine;
    private IEnumerator waitForAttackCoroutine;
    private IEnumerator switchToSwordCoroutine;
    private IEnumerator clearInventoryCoroutine;
    private bool stopMoving = false;
    private bool attackingPlayer = false;
    private EnemyInventoryScript enemyInventoryScript;
    private float coolDownTime = 0f;
    private bool dead = false;

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
        coolDownTime -= Time.fixedDeltaTime;
        //if player is attacking with sword or magic, draw sword
        if (PlayerAttacking())
        {
            attackingPlayer = true;
            agent.SetDestination(player.transform.position);
            //if enemy doesn't already have sword out, draw sword
            if (!this.GetComponent<Animator>().GetBool("HasSword"))
            {
                switchToSwordCoroutine = SwitchToSword(swordAnimTime);
                StartCoroutine(switchToSwordCoroutine);
            }

            //if you are close enough to the player, stop walking and attack
            if (Mathf.Abs((this.transform.position - player.transform.position).magnitude) < attackDistance && coolDownTime < 0)
            {
                this.GetComponent<Animator>().SetBool("Walking", false);
                agent.SetDestination(transform.position);
                stopMoving = true;
                this.GetComponent<Animator>().SetTrigger("Attack");
                coolDownTime = 5f;
                waitForAttackCoroutine = WaitForAttack(swordSlashAnimTime);
                StartCoroutine(waitForAttackCoroutine);
            }
        }
        else
        {
            attackingPlayer = false;
            //sheath sword
            if(this.GetComponent<Animator>().GetBool("HasSword"))
            {
                clearInventoryCoroutine = ClearInventory(clearAnimTime);
                StartCoroutine(clearInventoryCoroutine);
            }
            //if you reached the random position, find a new position to walk too
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

    public bool IsDead() { return dead; }

    //hit by player sword
    private void OnTriggerEnter(Collider other)
    {
        if (player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordAttack") &&
        other.gameObject.tag == "Sword" && !player.GetComponent<PlayerController>().IsDead())
        {
            Kill();
        }
    }

    //destroys enemy
    public void Kill()
    {
        dead = true;
        waitForDeathCoroutine = WaitForDeathAnim(deathAnimTime);
        StartCoroutine(waitForDeathCoroutine);
    }

    //finds a new random position to walk too
    public void updatePosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        target = hit.position;
        agent.SetDestination(target);
    }

    //checks if enemy arrived at set destination
    public bool ArrivedAtDestination()
    {
        if (agent.remainingDistance < 5f) { return true; }
        else { return false; }
    }

    //returns true if player has sword or wand drawn
    private bool PlayerAttacking()
    {
        if((player.GetComponent<Animator>().GetBool("HasSword") || player.GetComponent<Animator>().GetBool("HasMagic"))
            && Mathf.Abs((this.transform.position - player.transform.position).magnitude) < drawWeaponDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //wait for death animation before destroying
    private IEnumerator WaitForDeathAnim(float waitTime)
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
        agent.SetDestination(transform.position);
        stopMoving = true;
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    //wait for the attack animation
    private IEnumerator WaitForAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<Animator>().SetBool("Walking", true);
    }

    //switch to sword animation
    private IEnumerator SwitchToSword(float waitTime)
    {
        this.GetComponent<Animator>().SetBool("Walking", false);
        agent.SetDestination(transform.position);
        enemyInventoryScript.EquipSword();
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<Animator>().SetBool("Walking", true);
        agent.SetDestination(player.transform.position);
    }

    /*private IEnumerator SwitchToMagic(float waitTime)
    {
        this.GetComponent<Animator>().SetBool("Walking", false);
        agent.SetDestination(transform.position);
        enemyInventoryScript.EquipMagic();
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<Animator>().SetBool("Walking", true);
        agent.SetDestination(player.transform.position);
    }*/

    //stop walking and sheath sword
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
