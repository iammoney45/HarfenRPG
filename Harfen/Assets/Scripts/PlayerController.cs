using System.Collections;
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
    public GameObject lineRendererPrefab;
    private LineRenderer spellLine;

    private bool currentlyAttacking = false;
    private bool currentlyCasting = false;
    private IEnumerator waitForSpellCoroutine;
    private IEnumerator lineDestroyCoroutine;

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
        if(x > 0 || y > 0)
        {
            this.GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("Walking", false);
        }

        transform.Translate(playerSpeed * x * Time.fixedDeltaTime, 0f, playerSpeed * y * Time.fixedDeltaTime);

        if (Input.GetMouseButtonDown(0) && this.GetComponent<InventoryScript>().IsSwordEquipped())
        {
            currentlyAttacking = true;
            this.GetComponent<Animator>().SetTrigger("Attack");
        }
        else if(Input.GetMouseButtonDown(0) && this.GetComponent<InventoryScript>().IsMagicEquipped())
        {
            currentlyCasting = true;
            waitForSpellCoroutine = WaitForSpell(waitForMagicTime);
            StartCoroutine(waitForSpellCoroutine);
            
        }
        if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            currentlyAttacking = false;
        }
    }

    public bool IsCurrentlyAttacking() { return currentlyAttacking; }
    public bool IsCurrentlyCasting() { return currentlyCasting; }

    private void CastSpell()
    {
        RaycastHit spell;
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //set initial positon for line renderer and enable it
        spellLine.SetPosition(0, magicOrigin.transform.position);
        spellLine.enabled = true;
        //raycast
        if(Physics.Raycast(magicOrigin.transform.position, transform.forward, out spell, maxMagicDistance)){
            Debug.DrawLine(magicOrigin.transform.position, transform.forward, Color.green);
            //if it hits an enemy and raycast dist is less than max distance, kill enemy and draw raycast to hit point
            if(spell.collider.tag == "Enemy" && spell.distance < maxMagicDistance)
            {
                spell.collider.GetComponent<EnemyScript>().Kill();
                spellLine.SetPosition(1, spell.point);
            }
        }
        //if not hit anything, draw line length of max distancy
        else
        {
            spellLine.SetPosition(1, magicOrigin.transform.position + (transform.forward * maxMagicDistance));
        }
        //wait set time before destryoing line
        lineDestroyCoroutine = LineWait(lineDestoryTime);
        StartCoroutine(lineDestroyCoroutine);
    }

    private IEnumerator WaitForSpell(float waitTime)
    {
        this.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(waitTime);
        CastSpell();
    }

    private IEnumerator LineWait(float destroyTime)
    {
        spellLine.enabled = true;
        yield return new WaitForSeconds(destroyTime);
        spellLine.enabled = false;
    }
}
