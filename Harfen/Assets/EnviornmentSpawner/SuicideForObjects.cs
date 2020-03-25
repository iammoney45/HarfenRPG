using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideForObjects : MonoBehaviour
{
    //public Transform player = null;
    //public Component daddyScript = null;
    public GameObject player = null;


    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x - this.transform.position.x > 10 || player.transform.position.x + this.transform.position.x < 20){
        	player.GetComponent<CrownSpawner>().numSpawned--;
        	Destroy(this.gameObject);
        }
    }
}
