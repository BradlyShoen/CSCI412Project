using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batteryLogic : MonoBehaviour {
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			col.gameObject.GetComponent<playerLogic>().playerBattery = 100f;
			Destroy(gameObject);
		}
	}
}
