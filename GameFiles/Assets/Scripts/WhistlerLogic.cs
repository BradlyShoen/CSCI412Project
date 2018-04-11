using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhistlerLogic : MonoBehaviour {
	public GameObject whistleObject;
	public GameObject playerObject;
	public float maxDistanceToPlaySound = 50f;
	public float maxVolume = 0.5f;
	
	void Update () {
		if(Vector3.Distance(transform.position, playerObject.transform.position) <= maxDistanceToPlaySound){
			if((Vector3.Distance(transform.position, playerObject.transform.position)/maxDistanceToPlaySound) < maxVolume){
				whistleObject.GetComponent<AudioSource>().volume = Vector3.Distance(transform.position, playerObject.transform.position)/maxDistanceToPlaySound;
			}else{
				whistleObject.GetComponent<AudioSource>().volume = maxVolume;
			}
		}else{
			whistleObject.GetComponent<AudioSource>().volume = 0;
		}
	}
}
