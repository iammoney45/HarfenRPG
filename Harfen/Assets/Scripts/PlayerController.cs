using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public GameObject magicOrigin;
    public float maxMagicDistance;
    public Camera mainCamera;
    public float lineDestoryTime;

    private bool currentlyAttacking = false;
    private bool currentlyCasting = false;
    private LineRenderer spellLine;
    private IEnumerator lineDestroyCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        spellLine = GetComponent<LineRenderer>();
        spellLine.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.Translate(playerSpeed * x * Time.fixedDeltaTime, 0f, playerSpeed * y * Time.fixedDeltaTime);

        if (Input.GetMouseButtonDown(0) && this.GetComponent<InventoryScript>().IsSwordEquipped())
        {
            currentlyAttacking = true;
            this.GetComponent<Animator>().Play("Sword Stab");
        }
        else if(Input.GetMouseButtonDown(0) && this.GetComponent<InventoryScript>().IsMagicEquipped())
        {
            currentlyCasting = true;
            StartCoroutine("WaitForSpell");
            
        }
        if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            currentlyAttacking = false;
        }
        /*else if (!this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Magic"))
        {
            currentlyCasting = false;
        }*/
    }

    public bool IsCurrentlyAttacking() { return currentlyAttacking; }
    public bool IsCurrentlyCasting() { return currentlyCasting; }

    private void CastSpell()
    {
        RaycastHit spell;
        spellLine.SetPosition(0, magicOrigin.transform.position);
        spellLine.enabled = true;
        Vector3 rayOrgin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if(Physics.Raycast(magicOrigin.transform.position, transform.forward, out spell, maxMagicDistance)){
            Debug.DrawLine(magicOrigin.transform.position, transform.forward, Color.green);
            if(spell.collider.tag == "Enemy" && spell.distance < maxMagicDistance)
            {
                spell.collider.GetComponent<EnemyScript>().Kill();
                spellLine.SetPosition(1, spell.point);
            }
        }
        else
        {
            spellLine.SetPosition(1, magicOrigin.transform.position + (transform.forward * maxMagicDistance));
        }
        lineDestroyCoroutine = LineWait(lineDestoryTime);
        StartCoroutine(lineDestroyCoroutine);
    }

    private IEnumerator WaitForSpell()
    {
        this.GetComponent<Animator>().Play("Magic");
        yield return new WaitForSeconds(.5f);
        CastSpell();
    }

    private IEnumerator LineWait(float destroyTime)
    {
        spellLine.enabled = true;
        yield return new WaitForSeconds(destroyTime);
        spellLine.enabled = false;
    }
}
