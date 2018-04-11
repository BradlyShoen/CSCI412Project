using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagerLogic : MonoBehaviour {
	public GameObject playerObject;
	public GameObject whistlerObject;
	public GameObject batteryObject;
	
	void Start () {
		//Randomly spawn the whistler somewhere on the map
		whistlerObject.transform.position = new Vector3(Random.Range(-500f, 500f), transform.position.y, Random.Range(-500f, 500f));
	}
}
