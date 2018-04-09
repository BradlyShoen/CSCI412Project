using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAudioWithDelay : MonoBehaviour {
	public float timeBetweenSounds = 10.0f;
	AudioSource sound;
	
	void Start(){
		sound = GetComponent<AudioSource>();
	}
	
	void Update(){
		if(!sound.isPlaying){
			sound.PlayDelayed(timeBetweenSounds);
		}
	}
}
