using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab")
            && collision.gameObject.tag == "Sword")
        {
            Destroy(this.gameObject);
        }
    }
}
