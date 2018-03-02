using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRootScript : MonoBehaviour {
	
	GameObject player;

	void Start () {
		player = GameObject.FindWithTag("Player");
	}
	
	void Update () {
		gameObject.transform.localRotation = Quaternion.Euler(0f,-player.transform.rotation.y,0f);
	}
}
