using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScript : MonoBehaviour {
	public GameObject endMessage;
	
	void Update () {
		if(!GetComponent<Animation>().isPlaying){
			endMessage.SetActive(true);
		}
	}
}
