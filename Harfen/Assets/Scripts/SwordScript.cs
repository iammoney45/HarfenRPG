using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject player;

    private float cooldown = 2; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown -= Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && 
        cooldown <= 0 &&
        player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            collision.gameObject.GetComponent<EnemyScript>().Kill();
            cooldown = 2;
        }
;    }
}
