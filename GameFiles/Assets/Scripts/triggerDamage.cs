using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDamage : MonoBehaviour {
	public float damageToTake = 5.0f;
	public float rate = 1.0f;

	public void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "Player"){
            StartCoroutine(applyPlayerDamage(col.gameObject));
		}
	}
	
	public IEnumerator applyPlayerDamage(GameObject player){
		player.GetComponent<playerLogic>().TakeDamage(damageToTake * rate * Time.deltaTime);
		yield return 0;
	}
}
