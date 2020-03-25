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

    private bool swordEquipped = false;
    private GameObject swordGO;
    private bool magicEquipped = false;
    private bool swordMagicEqupped = false;

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
            //destroy sword if it exists and set all bools to false
            if (swordGO != null) { Destroy(swordGO); }
            swordEquipped = false;
            magicEquipped = false;
            swordMagicEqupped = false;
            currentUIImage.texture = emptyUIImage;
        }
        //equip sword
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !swordEquipped)
        {
            //don't spawn two swords
            if (swordGO != null) { Destroy(swordGO); }
            //if magic "anim" is still playing
            //if (magicEquipped) { this.GetComponent<Animator>().Play("Idle"); }

            //instantiate sword at hand
            swordGO = Instantiate(sword) as GameObject;
            swordGO.transform.SetParent(hand.transform);
            swordGO.transform.localPosition = swordPosition;
            swordGO.transform.localEulerAngles = swordRotation;
            swordGO.GetComponent<SwordScript>().player = this.gameObject;

            //set bools/UI
            swordEquipped = true;
            magicEquipped = false;
            swordMagicEqupped = false;
            currentUIImage.texture = swordUIImage;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && !magicEquipped)
        {
            //Destroy sword if it exists
            if (swordGO != null) { Destroy(swordGO); }
            //play magic "anim"
            //this.GetComponent<Animator>().Play("Magic");

            //set bools/UI
            swordEquipped = false;
            magicEquipped = true;
            swordMagicEqupped = false;
            currentUIImage.texture = magicUIImage;
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
                swordGO.GetComponent<SwordScript>().player = this.gameObject;
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
}
