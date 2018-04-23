using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDeathScript : MonoBehaviour {
	public GameObject deathUI;
	public GameObject whistlerObject;
	public GameObject mainCamera;
	
	void Update(){
		if(whistlerObject.GetComponent<Animator>().isInitialized){
			StartCoroutine(returnToMenu());
		}
	}
	
	void Awake(){
		mainCamera.GetComponent<Animation>().Play();
	}
 
	IEnumerator returnToMenu(){
		yield return new WaitForSeconds(2);
		deathUI.SetActive(true);
	}
}
