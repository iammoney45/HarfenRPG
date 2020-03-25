using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed;
    public GameObject magicOrigin;
    public float maxMagicDistance;

    private bool currentlyAttacking = false;
    private bool currentlyCasting = false;

    // Start is called before the first frame update
    void Start()
    {

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
            this.GetComponent<Animator>().Play("Magic");
            
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

        /*RaycastHit spell = new RaycastHit();
        if(Physics.Linecast(magicOrigin.transform, mouse.transform, out spell)){
            Debug.DrawLine(transform.position, mouse.position, Color.green);
            if(spell.collider.tag == "Enemy" && spell.distance < maxMagicDistance)
            {
                print("Spell Hit");
            }
        }*/
    }

}
