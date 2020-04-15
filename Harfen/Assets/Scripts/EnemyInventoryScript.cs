using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInventoryScript : MonoBehaviour
{

    public Vector3 swordPosition;
    public Vector3 swordRotation;
    public Vector3 wandPosition;
    public Vector3 wandRotation;
    public GameObject sword;
    public GameObject wand;
    public GameObject hand;
    public GameObject arm;
    public float swordDrawTime; //when idling
    public float swordSheathTime; //for destroying sword
    public float magicDrawTime; //when idling
    public float magicDrawTime2; //switching sword to magic

    private GameObject swordGO;
    private GameObject wandGO;
    private bool swordEquipped = false;
    private bool magicEquipped = false;
    private bool swordMagicEqupped = false;
    private IEnumerator equipSwordCoroutine;
    private IEnumerator destroyObjectCoroutine;
    private IEnumerator equipMagicCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipSword()
    {
        //set bools/UI
        swordEquipped = true;
        magicEquipped = false;
        swordMagicEqupped = false;

        if (wandGO != null)
        {
            destroyObjectCoroutine = DestroyObject(swordSheathTime);
            StartCoroutine(destroyObjectCoroutine);
        }
        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            this.GetComponent<Animator>().SetBool("HasSword", true);
            this.GetComponent<Animator>().SetBool("HasMagic", false);
            equipSwordCoroutine = EquipSword(swordDrawTime);
            StartCoroutine(equipSwordCoroutine);
        }
        //if switching from magic to sword don't "sheath" magic to redraw sword and must be in idle magic state
        else if (this.GetComponent<Animator>().GetBool("HasMagic") &&
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MagicIdle"))
        {
            this.GetComponent<Animator>().SetBool("SwitchMagicToSword", true);
            equipSwordCoroutine = EquipSword(swordDrawTime);
            StartCoroutine(equipSwordCoroutine);
        }
    }

    /*public void EquipMagic()
    {
        //set bools/UI
        swordEquipped = false;
        magicEquipped = true;
        swordMagicEqupped = false;

        //Destroy sword if it exists
        if (swordGO != null)
        {
            destroyObjectCoroutine = DestroyObject(swordSheathTime);
            StartCoroutine(destroyObjectCoroutine);
        }

        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            this.GetComponent<Animator>().SetBool("HasSword", false);
            this.GetComponent<Animator>().SetBool("HasMagic", true);
            equipMagicCoroutine = EquipMagic(magicDrawTime);
            StartCoroutine(equipMagicCoroutine);
        }
        //if switching from magic to sword don't "sheath" magic to redraw sword and must be in idle magic state
        if (this.GetComponent<Animator>().GetBool("HasSword") &&
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordIdle"))
        {
            equipMagicCoroutine = EquipMagic(magicDrawTime + magicDrawTime2);
            StartCoroutine(equipMagicCoroutine);
        }
    }*/

    public void ClearInventory()
    {
        //set bools/UI
        swordEquipped = false;
        magicEquipped = false;
        swordMagicEqupped = false;

        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordIdle") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MagicIdle"))
        {
            //destroy sword if it exists and set all bools to false
            if (swordGO != null || wandGO != null)
            {
                destroyObjectCoroutine = DestroyObject(swordSheathTime);
                StartCoroutine(destroyObjectCoroutine);
                this.GetComponent<Animator>().SetBool("HasMagic", false);
                this.GetComponent<Animator>().SetBool("HasSword", false);
            }
        }
    }

    public bool IsSwordEquipped() { return swordEquipped; }
    public bool IsMagicEquipped() { return magicEquipped; }

    private IEnumerator EquipSword(float animTime)
    {
        //wait for animation to play
        yield return new WaitForSeconds(animTime);

        //set animator bools
        this.GetComponent<Animator>().SetBool("HasMagic", false);
        this.GetComponent<Animator>().SetBool("HasSword", true);
        this.GetComponent<Animator>().SetBool("SwitchMagicToSword", false);
        if (!swordGO)
        {
            //instantiate sword at hand
            swordGO = Instantiate(sword) as GameObject;
            swordGO.transform.SetParent(hand.transform);
            swordGO.transform.localPosition = swordPosition;
            swordGO.transform.localEulerAngles = swordRotation;
        }

    }

    /*private IEnumerator EquipMagic(float animTime)
    {
        //wait for animation to play
        yield return new WaitForSeconds(animTime);

        //set animator bools
        this.GetComponent<Animator>().SetBool("HasMagic", true);
        this.GetComponent<Animator>().SetBool("HasSword", false);

        wandGO = Instantiate(wand) as GameObject;
        wandGO.transform.SetParent(hand.transform);
        wandGO.transform.localPosition = wandPosition;
        wandGO.transform.localEulerAngles = wandRotation;
    }*/

    private IEnumerator DestroyObject(float animTime)
    {
        //wait for animation to play
        yield return new WaitForSeconds(animTime);

        if (swordGO != null) { Destroy(swordGO); }
        if (wandGO != null) { Destroy(wandGO); }
    }
}
