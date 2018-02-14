using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class playerLogic : MonoBehaviour {
	
	public GameObject mainCanvas;
	public GameObject pausePanel;
	
	private GameObject playerObject;
	private playerController playerMover;
	private bool currentlyPaused = false;

	void Start () {
		playerObject = GameObject.FindWithTag("Player");
		playerMover = playerObject.GetComponent<playerController>();
		mainCanvas = GameObject.Find("MainCanvas");
		
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
		if(Input.GetKeyDown("escape")){
			PauseUnpause();
		}
		
		if(currentlyPaused){
			pausePanel.SetActive(true);
			
			playerMover.enabled = false;
			Time.timeScale = 0.0f;
			
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
		}else{
			pausePanel.SetActive(false);
			
			playerMover.enabled = true;
			Time.timeScale = 1.0f;
			
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			
		}
	}
	
	public void RestartCurrentScene(){
		Scene loadedLevel = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (loadedLevel.buildIndex);
	}
	
	public void ExitGame(){
		Application.Quit();
	}
	
	public void PauseUnpause(){
		if(currentlyPaused){
			currentlyPaused = false;
		}else{
			currentlyPaused = true;
		}
	}
}
