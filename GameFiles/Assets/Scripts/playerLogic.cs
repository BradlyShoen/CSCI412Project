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
	
	public float playerHealth = 100f;
	public float playerStamina = 100f;
	public float staminaIncreaseRate = 50.0f;
	public float runningDecreaseRate = 10.0f;
	
	private const float PLAYERMAXHEALTH = 100f;
	private const float PLAYERMAXSTAMINA = 100f;
	

	void Start () {
		playerObject = GameObject.FindWithTag("Player");
		playerMover = playerObject.GetComponent<playerController>();
		mainCanvas = GameObject.Find("MainCanvas");

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
		if(Input.GetKeyDown("`")){
			PauseUnpause();
		}
		
		if(currentlyPaused){
			pausePanel.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;

			playerMover.enabled = false;
			Time.timeScale = 0.0f;
		}else{
			pausePanel.SetActive(false);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			playerMover.enabled = true;
			Time.timeScale = 1.0f;
			
		}
		
		if(playerHealth > PLAYERMAXHEALTH){
			playerHealth = PLAYERMAXHEALTH;
		}else if(playerHealth < 0){
			playerHealth = 0;
		}
		
		if(playerStamina > PLAYERMAXSTAMINA){
			playerStamina = PLAYERMAXSTAMINA;
		}else if(playerStamina < 0){
			playerStamina = 0;
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

	public void TakeDamage(float dmg){
		playerHealth = playerHealth - dmg;
	}
	
	public IEnumerator decreaseStamina(){
		playerStamina -=  runningDecreaseRate * Time.deltaTime;
		yield return 0;
	}
	
	public IEnumerator increaseStamina(){
		playerStamina +=  staminaIncreaseRate * Time.deltaTime;
		yield return 0;
	}
}
