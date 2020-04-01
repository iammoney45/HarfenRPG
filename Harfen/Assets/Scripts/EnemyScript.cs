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

    private void OnCollisionEnter(Collision collision)
    {
        print("hit by: " + collision.gameObject.tag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(player.GetComponent<PlayerController>().GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Sword Stab"))
        {
            Kill();
        }

    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
