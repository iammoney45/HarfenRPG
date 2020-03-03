using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{

    public Vector3 swordPosition;
    public Vector3 swordRotation;
    public GameObject sword;
    public GameObject hand;
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
    void Update()
    {
        //remove sword (or current item in future)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(swordGO);
            swordEquipped = false;
            magicEquipped = false;
            swordMagicEqupped = false;
            currentUIImage.texture = emptyUIImage;
        }
        //equip sword
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !swordEquipped)
        {
            swordGO = Instantiate(sword) as GameObject;
            swordGO.transform.SetParent(hand.transform);
            swordGO.transform.localPosition = swordPosition;
            swordGO.transform.localEulerAngles = swordRotation;
            swordEquipped = true;
            currentUIImage.texture = swordUIImage;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && !magicEquipped)
        {
            magicEquipped = true;
            currentUIImage.texture = magicUIImage;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && !swordMagicEqupped)
        {
            if(!swordEquipped)
            {
                swordGO = Instantiate(sword) as GameObject;
                swordGO.transform.SetParent(hand.transform);
                swordGO.transform.localPosition = swordPosition;
                swordGO.transform.localEulerAngles = swordRotation;
            }
            swordEquipped = true;
            magicEquipped = true;
            swordMagicEqupped = true;
            currentUIImage.texture = magicAndSwordUIImage;

        }
    }

    public bool IsSwordEquipped() { return swordEquipped; }
}
