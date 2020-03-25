using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideForObjects : MonoBehaviour
{
    //public Transform player = null;
    //public Component daddyScript = null;
    public GameObject player = null;
    int timerOfDeath = 0;


    void Awake()
    {
    	int layerMask = 1 << 9;
    	//layerMask = ~layerMask;
    	RaycastHit hit;
    	if(Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)){
    		if(hit.distance > 0.3){
    			Vector3 newPos = this.transform.position;
    			newPos.y -= hit.distance;
    			this.transform.position = newPos;
    			float turn = Random.Range(0f, 2f);
    			if(turn < 1){
    				this.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
    			}
    			//this.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f)));
    		}
    	}
    	else if(Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask)){
    		if(hit.distance > 0.3){
    			Vector3 newPos = this.transform.position;
    			newPos.y += hit.distance;
    			this.transform.position = newPos;
    		}
    	}
    	else{
    		print("missing bruv");
    	}
    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(player.transform.position, this.transform.position) > 10 || timerOfDeath > Random.Range(500f, 1500f)){
        	player.GetComponent<CrownSpawner>().numSpawned--;
    		Destroy(this.gameObject);
        }
        else{
        	timerOfDeath++;
        	
        }
    }
}
