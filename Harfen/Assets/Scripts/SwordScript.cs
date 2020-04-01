using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject player;
    public Vector3 pos;
    public Vector3 rot;

    private float cooldown = 2; 

    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.localPosition;
        rot = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown -= Time.fixedDeltaTime;
        this.transform.localPosition = pos;
        this.transform.localEulerAngles = rot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Hit");
        if(collision.gameObject.tag == "Enemy" && 
        cooldown <= 0 &&
        player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            print("Kill");
            collision.gameObject.GetComponent<EnemyScript>().Kill();
            cooldown = 2;
        }
;    }
}
