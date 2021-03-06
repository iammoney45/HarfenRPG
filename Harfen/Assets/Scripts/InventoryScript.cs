﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{

    public Vector3 swordPosition;
    public Vector3 swordRotation;
    public Vector3 wandPosition;
    public Vector3 wandRotation;
    public GameObject sword;
    public GameObject wand;
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
        currentUIImage.texture = emptyUIImage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //remove sword (or current item in future)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MagicIdle") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordIdle"))
            {
                this.GetComponent<Animator>().SetBool("HasMagic", false);
                this.GetComponent<Animator>().SetBool("HasSword", false);

                //destroy sword if it exists and set all bools to false
                if (swordGO != null || wandGO != null)
                {
                    destroyObjectCoroutine = DestroyObject(swordSheathTime);
                    StartCoroutine(destroyObjectCoroutine);
                }

                swordEquipped = false;
                magicEquipped = false;
                swordMagicEqupped = false;
                currentUIImage.texture = emptyUIImage;
            }
        }
        //equip sword
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !swordEquipped)
        {
            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("MagicIdle"))
            {
                //set bools/UI
                swordEquipped = true;
                magicEquipped = false;
                swordMagicEqupped = false;
                currentUIImage.texture = swordUIImage;

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
        }
        //equip magic
        else if(Input.GetKeyDown(KeyCode.Alpha2) && !magicEquipped)
        {
            if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordIdle")) {
                //set bools/UI
                swordEquipped = false;
                magicEquipped = true;
                swordMagicEqupped = false;
                currentUIImage.texture = magicUIImage;

                //Destroy sword if it exists
                if (swordGO != null)
                {
                    destroyObjectCoroutine = DestroyObject(swordSheathTime);
                    StartCoroutine(destroyObjectCoroutine);
                }

                if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    this.GetComponent<Animator>().SetBool("HasMagic", true);
                    this.GetComponent<Animator>().SetBool("HasSword", false);
                    equipMagicCoroutine = EquipMagic(magicDrawTime);
                    StartCoroutine(equipMagicCoroutine);
                }
                if (this.GetComponent<Animator>().GetBool("HasSword") &&
                    this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordIdle"))
                {
                    this.GetComponent<Animator>().SetBool("HasMagic", true);
                    this.GetComponent<Animator>().SetBool("HasSword", false);
                    equipMagicCoroutine = EquipMagic(magicDrawTime + magicDrawTime2);
                    StartCoroutine(equipMagicCoroutine);
                }
            }
        }
        //for magic and sword. to be implemented later
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
    }

    private IEnumerator DestroyObject(float animTime)
    {
        //wait for animation to play
        yield return new WaitForSeconds(animTime);
        if(swordGO != null) { Destroy(swordGO); }
        if(wandGO != null) { Destroy(wandGO); }
    }

    private IEnumerator EquipMagic(float animTime)
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
    }
}
