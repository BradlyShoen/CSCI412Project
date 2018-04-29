using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightItemsLogic : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<playerLogic>().ItemsCollectedCount += 1;
            col.gameObject.GetComponent<playerLogic>().SetCountText();
			if (!GameObject.Find ("ItemFound").GetComponent<AudioSource> ().isPlaying) { 
				GameObject.Find ("ItemFound").GetComponent<AudioSource> ().Play ();
			}
            Destroy(gameObject);
        }
    }
}
