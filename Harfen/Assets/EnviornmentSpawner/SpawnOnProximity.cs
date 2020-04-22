using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnProximity : MonoBehaviour
{
	public GameObject playerTarget = null;
	public GameObject childToSpawn = null;
	private bool spawned = false;

    // Update is called once per frame
    void Update()
    {
        if(!spawned && Vector3.Distance(playerTarget.transform.position, this.transform.position) > 15){
        	childToSpawn.SetActive(true);
        }
    }
}
