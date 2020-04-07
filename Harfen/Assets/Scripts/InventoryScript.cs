using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{

    public Vector3 swordPosition;
    public Vector3 swordRotation;
    public Vector3 armRotation;
    public GameObject sword;
    public GameObject hand;
    public GameObject arm;
    public Texture emptyUIImage;
    public Texture swordUIImage;
    public Texture magicUIImage;
    public Texture magicAndSwordUIImage;
    public RawImage currentUIImage;
    public float swordDrawTime; //when idling
    public float swordSheathTime; //for destroying sword
    public float magicDrawTime; //when idling
    public float magicDrawTime2; //switching sword to magic

    private bool swordEquipped = false;
    private GameObject swordGO;
    private bool magicEquipped = false;
    private bool swordMagicEqupped = false;
    private IEnumerator equipSwordCoroutine;
    private IEnumerator destroySwordCoroutine;
    private IEnumerator equipMagicCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        currentUIImage.texture = emptyUIImage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //remove sword (or current item in future)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.GetComponent<Animator>().SetBool("HasMagic", false);
            this.GetComponent<Animator>().SetBool("HasSword", false);

            //destroy sword if it exists and set all bools to false
            if (swordGO != null)
            {
                destroySwordCoroutine = DestroySword(swordSheathTime);
                StartCoroutine(destroySwordCoroutine);
            }

            swordEquipped = false;
            magicEquipped = false;
            swordMagicEqupped = false;
            currentUIImage.texture = emptyUIImage;
        }
        //equip sword
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !swordEquipped)
        {
            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                this.GetComponent<Animator>().SetBool("HasSword", true);
                equipSwordCoroutine = EquipSword(swordDrawTime);
                StartCoroutine(equipSwordCoroutine);
            }
            //if switching from magic to sword don't "sheath" magic to redraw sword and must be in idle magic state
            if(this.GetComponent<Animator>().GetBool("HasMagic") && 
                this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MagicIdle"))
            {
                this.GetComponent<Animator>().SetBool("SwitchMagicToSword", true);
                equipSwordCoroutine = EquipSword(swordDrawTime);
                StartCoroutine(equipSwordCoroutine);
            }

            //don't spawn two swords
            if (swordGO != null) { Destroy(swordGO); }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && !magicEquipped)
        {
            //Destroy sword if it exists
            if (swordGO != null) 
            {
                destroySwordCoroutine = DestroySword(swordSheathTime);
                StartCoroutine(destroySwordCoroutine);
            }

            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
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

            //set animator bools
            this.GetComponent<Animator>().SetBool("HasMagic", true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && !swordMagicEqupped)
        {
            //if sword is not equipped, equip it
            if(!swordEquipped)
            {
                swordGO = Instantiate(sword) as GameObject;
                swordGO.transform.SetParent(hand.transform);
                swordGO.transform.localPosition = swordPosition;
                swordGO.transform.localEulerAngles = swordRotation;
            }
            //if magic "anim" is still playing
            //if (magicEquipped) { this.GetComponent<Animator>().Play("Idle"); }

            //set bools/UI
            swordEquipped = false;
            magicEquipped = false;
            swordMagicEqupped = true;
            currentUIImage.texture = magicAndSwordUIImage;

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

        //instantiate sword at hand
        swordGO = Instantiate(sword) as GameObject;
        swordGO.transform.SetParent(hand.transform);
        swordGO.transform.localPosition = swordPosition;
        swordGO.transform.localEulerAngles = swordRotation;

        //set bools/UI
        swordEquipped = true;
        magicEquipped = false;
        swordMagicEqupped = false;
        currentUIImage.texture = swordUIImage;
    }

    private IEnumerator DestroySword(float animTime)
    {
        //wait for animation to play
        yield return new WaitForSeconds(animTime);

        Destroy(swordGO);
    }

    private IEnumerator EquipMagic(float animTime)
    {
        //set animator bools
        this.GetComponent<Animator>().SetBool("HasMagic", true);
        this.GetComponent<Animator>().SetBool("HasSword", false);

        //wait for animation to play
        yield return new WaitForSeconds(animTime);

        //set bools/UI
        swordEquipped = false;
        magicEquipped = true;
        swordMagicEqupped = false;
        currentUIImage.texture = magicUIImage;
    }
}
