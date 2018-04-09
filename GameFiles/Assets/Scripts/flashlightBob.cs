using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightBob: MonoBehaviour {

	private float timer = 0.0f;
	public float bobbingSpeed = 0.18f;
	private float runningBobSpeed;
	private float walkingBobSpeed;
	public float bobbingAmount = 0.02f;
	public float midpoint = 0.0f;
    public float intensity = 0;
    private playerLogic playerLogic;

	void Start(){

        playerLogic = GameObject.FindWithTag("Player").GetComponent<playerLogic>();
        walkingBobSpeed = bobbingSpeed;
		runningBobSpeed = 1.5f * bobbingSpeed;
	}

	void Update () {
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 cSharpConversion = transform.localPosition; 

		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
			timer = 0.0f;
		}else {
			if (Input.GetKey(KeyCode.LeftShift)) {
				bobbingSpeed = runningBobSpeed;
			} else {
				bobbingSpeed = walkingBobSpeed;
			}
			waveslice = Mathf.Sin(timer);
			timer = timer + bobbingSpeed;
			if (timer > Mathf.PI * 2) {
				timer = timer - (Mathf.PI * 2);
			}
		}
		if (waveslice != 0) {
			float translateChange = waveslice * bobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			cSharpConversion.y = midpoint + translateChange;
		}else {
			cSharpConversion.y = midpoint;
		}

		transform.localPosition = cSharpConversion;
	}
}
