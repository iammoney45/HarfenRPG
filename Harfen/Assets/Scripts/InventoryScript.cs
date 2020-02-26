using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    public Vector3 swordPosition;
    public Vector3 swordRotation;
    public GameObject sword;
    public GameObject hand;

    private bool swordEquipped = false;
    private GameObject swordGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !swordEquipped)
        {
            print("Yep");swordGO = Instantiate(sword) as GameObject;
            swordGO.transform.SetParent(hand.transform);
            swordGO.transform.localPosition = swordPosition;
            swordGO.transform.localEulerAngles = swordRotation;
            swordEquipped = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(swordGO);
            swordEquipped = false;
        }
    }
}
