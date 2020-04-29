using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownSpawner : MonoBehaviour
{
	public int numSpawned = 0;
	public GameObject Grass = null;
	public GameObject Butterfly = null;
	public Transform PlayerTransform = null;

    // Update is called once per frame
    void Update()
    {
     	if(numSpawned < 100){
     		Spawn(Grass);
     		numSpawned++;
     	}
    }

    void Spawn(GameObject spawn){
    	//print("spawning");
    	int layerMask = 1 << 8;
    	layerMask = ~layerMask;
    	RaycastHit hit;
    	Vector3 targetPoint = PlayerTransform.position;
    	float posX = Random.Range(-8.0f, 8.0f);
    	float posZ = Random.Range(-8.0f, 8.0f);
    	
    	//print(targetPoint);
    	if(Physics.Raycast(PlayerTransform.position, transform.TransformDirection(Vector3.down), out hit, 50, layerMask)){

    	}
    	else{
    		print("no hit");
    	}
    	Vector3 NewPos = hit.point;
    	NewPos.x += posX;
    	NewPos.z += posZ;
    	NewPos.y += 5;
    	//print(NewPos);
        float randRotation = UnityEngine.Random.Range(0f, 360f);
    	GameObject spawny = Instantiate(spawn, NewPos, Quaternion.Euler(new Vector3(0,randRotation,0)));
    	//spawny.GetComponent<SuicideForObjects>().player = PlayerTransform;
    	//spawny.GetComponent<SuicideForObjects>().daddyScript = this;
        float randScale = UnityEngine.Random.Range(4f, 10f);
        //spawny.transform.localScale = new Vector3(randScale, randScale, randScale);
    	spawny.GetComponent<SuicideForObjects>().player = this.gameObject;

    }
}
