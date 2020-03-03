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
     	if(numSpawned < 50){
     		Spawn(Grass);
     		numSpawned++;
     	}
    }

    void Spawn(GameObject spawn){
    	float posX = Random.Range(-5.0f, 5.0f);
    	float posY = Random.Range(-5.0f, 5.0f);
    	float posZ = Random.Range(-5.0f, 5.0f);
    	Vector3 NewPos = new Vector3(posX,posY,posZ);
    	GameObject spawny = Instantiate(spawn, PlayerTransform.position + NewPos, Quaternion.Euler(new Vector3(Random.Range(0f,360f),Random.Range(0f,360f),Random.Range(0f,360f))));
    	//spawny.GetComponent<SuicideForObjects>().player = PlayerTransform;
    	//spawny.GetComponent<SuicideForObjects>().daddyScript = this;
    	spawny.GetComponent<SuicideForObjects>().player = this.gameObject;
    }
}
