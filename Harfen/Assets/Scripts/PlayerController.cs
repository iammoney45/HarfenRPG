﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public GameObject magicOrigin;
    public float maxMagicDistance;
    public Camera mainCamera;
    public float waitForMagicTime;
    public float lineDestoryTime;
    public float deathAnimTime;
    public GameObject lineRendererPrefab;

    private LineRenderer spellLine;
    private bool currentlyAttacking = false;
    private bool currentlyCasting = false;
    private IEnumerator waitForSpellCoroutine;
    private IEnumerator lineDestroyCoroutine;
    private IEnumerator waitForDeathCoroutine;
    private float coolDownTime = 0f;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        spellLine = lineRendererPrefab.GetComponent<LineRenderer>();
        spellLine.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        coolDownTime -= Time.fixedDeltaTime;

        if(System.Math.Abs(x) > Mathf.Epsilon || System.Math.Abs(y) > Mathf.Epsilon)
        {
            this.GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("Walking", false);
        }

        transform.Translate(playerSpeed * x * Time.fixedDeltaTime, 0f, playerSpeed * y * Time.fixedDeltaTime);

        if (Input.GetMouseButtonDown(0) && this.GetComponent<InventoryScript>().IsSwordEquipped() && coolDownTime < 0f)
        {
            currentlyAttacking = true;
            this.GetComponent<Animator>().SetTrigger("Attack");
            coolDownTime = 5f;
        }
        else if(Input.GetMouseButtonDown(0) && this.GetComponent<InventoryScript>().IsMagicEquipped() && coolDownTime < 0f)
        {
            currentlyCasting = true;
            waitForSpellCoroutine = WaitForSpell(waitForMagicTime);
            StartCoroutine(waitForSpellCoroutine);
            coolDownTime = 5f;

        }
    }

    public bool IsCurrentlyAttacking() { return currentlyAttacking; }
    public bool IsCurrentlyCasting() { return currentlyCasting; }
    public bool IsDead() { return dead; }

    private void CastSpell()
    {
        RaycastHit spell;
        //set initial positon for line renderer and enable it
        spellLine.SetPosition(0, magicOrigin.transform.position);
        //raycast
        Debug.DrawLine(magicOrigin.transform.position, transform.forward*maxMagicDistance + magicOrigin.transform.position, Color.green);
        if (Physics.Raycast(magicOrigin.transform.position, transform.forward, out spell, maxMagicDistance)){
            //if it hits an enemy and raycast dist is less than max distance, kill enemy and draw raycast to hit point
            if(spell.collider.tag == "Enemy" && !spell.collider.GetComponent<EnemyScript>().IsDead())
            {
                spell.collider.GetComponent<EnemyScript>().Kill();
                spellLine.SetPosition(1, spell.point);
            }
        }
        //if not hit anything, draw line length of max distanc
        else
        {
            spellLine.SetPosition(1, (transform.forward * maxMagicDistance + magicOrigin.transform.position));
        }
        //wait set time before destryoing line
        lineDestroyCoroutine = LineWait(lineDestoryTime);
        StartCoroutine(lineDestroyCoroutine);
    }

    //if hit by enemy with sword
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordAttack") &&
            other.gameObject.tag == "Sword" && !dead &&
            other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.tag != "Player")
        {
            Kill();
        }

    }

    //kills player
    public void Kill()
    {
        dead = true;
        waitForDeathCoroutine = WaitForDeathAnim(deathAnimTime);
        StartCoroutine(waitForDeathCoroutine);
    }

    //wait for death animation before destroying
    private IEnumerator WaitForDeathAnim(float waitTime)
    {
        this.GetComponent<Animator>().SetTrigger("Dead");
        this.GetComponent<Animator>().SetBool("Walking", false);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    //wait for spell animation before actually casting
    private IEnumerator WaitForSpell(float waitTime)
    {
        this.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(waitTime);
        CastSpell();
    }

    //wait destroyTime before destroying the magic line
    private IEnumerator LineWait(float destroyTime)
    {
        spellLine.enabled = true;
        yield return new WaitForSeconds(destroyTime);
        spellLine.enabled = false;
    }
}
