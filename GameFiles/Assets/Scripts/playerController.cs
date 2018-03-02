//This script is originally created by Unity but heavly modified to fit our player's controls

using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson{
	[RequireComponent(typeof (Rigidbody))]
	[RequireComponent(typeof (CapsuleCollider))]
	public class playerController : MonoBehaviour{
		[Serializable]
		public class MovementSettings{
			public float ForwardSpeed = 3.5f;   // Speed when walking forward
			public float BackwardSpeed = 3.5f;  // Speed when walking backwards
			public float StrafeSpeed = 3.5f;    // Speed when walking sideways
			public float CrouchMultiplier = 0.35f; //Speed when crouching
			public float RunMultiplier = 2.0f;   // Speed when sprinting
			public KeyCode RunKey = KeyCode.LeftShift;
			public float JumpForce = 45f;
			
			public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
			[HideInInspector] public float CurrentTargetSpeed = 8f;
			public playerLogic playerStats;

			private bool m_Running;
			[HideInInspector] public bool m_Crouching, m_IsGrounded;
			
			//This function updates the speed of the player based on whether or not hes crouching, running, strafing, etc
			public void UpdateDesiredTargetSpeed(Vector2 input){
				if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0){
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0){
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0){
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}
				
				if (Input.GetKey(RunKey) && playerStats.playerStamina > 0.0f && m_IsGrounded)
				{
					CurrentTargetSpeed *= RunMultiplier;
					m_Running = true;
				}else{
					m_Running = false;
				}
				
				if (m_Crouching){
					CurrentTargetSpeed *= CrouchMultiplier;
				}
				
			}
			
			//This function returns the protected variable m_Running
			public bool Running{
				get { return m_Running; }
			}
		}


		[Serializable]
		public class AdvancedSettings
		{
			public float groundCheckDistance = 0.1f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
			public float stickToGroundHelperDistance = 0.6f; // stops the character
			public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
			public bool airControl = true; // can the user control the direction that is being moved in the air
			[Tooltip("set it to 0.1 or more if you get stuck in wall")]
			public float shellOffset = 0.1f; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
		}


		public Camera cam;
		public MovementSettings movementSettings = new MovementSettings();
		public MouseLook mouseLook = new MouseLook();
		public AdvancedSettings advancedSettings = new AdvancedSettings();
		
		public float slopeLimit = 50f;


		private Rigidbody m_RigidBody;
		private CapsuleCollider m_Capsule;
		private float m_YRotation;
		private Vector3 m_GroundContactNormal;
		private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_Crouching;
		private bool m_IsGrounded;
		private playerLogic playerStats;
		private bool m_PreviouslyCrouched, m_checkCrouch;
		

		//Returns the velocity Vector3 of our player
		public Vector3 Velocity{
			get { return m_RigidBody.velocity; }
		}
		
		//Returns whether or not our player is grounded
		public bool Grounded{
			get { return m_IsGrounded; }
		}

		//Returns whether or not our player is jumping
		public bool Jumping{
			get { return m_Jumping; }
		}
		
		//Returns whether or not our player is running
		public bool Running{
			get
			{ return movementSettings.Running; }
		}

		//Initializes our variables
		private void Start()
		{
			m_RigidBody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			mouseLook.Init (transform, cam.transform);
			playerStats = GetComponent<playerLogic>();
		}

		//Gets mouse input, checks if we are jumping, and also checks if we are crouching
		private void Update()
		{
			RotateView();

			if (Input.GetButtonDown("Jump") && !m_Jump && NothingAbove())
			{
				m_Jump = true;
			}
			
			if(Input.GetButton("Crouch")){
				m_PreviouslyCrouched = true;
				movementSettings.m_Crouching = true;
				m_Crouching = true;
				m_checkCrouch = true;
			}else{
				if(NothingAbove()){
					movementSettings.m_Crouching = false;
					m_Crouching = false;
					if(m_checkCrouch){
						StartCoroutine(setPreviouslyCrouched());
						m_checkCrouch = false;
					}
				}
			}
			
		}


		private void FixedUpdate()
		{
			GroundCheck();
			Vector2 input = GetInput();

			if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
			{
				// always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = transform.forward*input.y + transform.right*input.x;
				desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

				desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
				desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
				desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
				if (m_RigidBody.velocity.sqrMagnitude <
					(movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
				{
					m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
				}
			}

			if (m_IsGrounded)
			{
				m_RigidBody.drag = 5f;

				if (m_Jump)
				{
					m_RigidBody.drag = 0f;
					m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
					m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
					m_Jumping = true;
				}

				if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
				{
					m_RigidBody.Sleep();
				}
			}
			else
			{
				m_RigidBody.drag = 0f;
				if (m_PreviouslyGrounded && !m_Jumping)
				{
					StickToGroundHelper();
				}
			}
			m_Jump = false;
			
			//MODIFIED PORTION STARTS HERE
			
			//This changes the height of our character based on whether or not we are crouching
			if(m_Crouching){
				gameObject.GetComponent<CapsuleCollider> ().height = 1.0f;
			}else{
				gameObject.GetComponent<CapsuleCollider> ().height = 1.6f;
			}
			
			//This decreases or increases our stamina based on whether or not we are standing still or running
			if(Running && input != new Vector2(0f,0f)){
				StartCoroutine(playerStats.decreaseStamina());
			}else if(input == new Vector2(0f,0f)){
				StartCoroutine(playerStats.increaseStamina());
			}
			
			RaycastHit hit;
			Physics.Raycast(transform.position, -Vector3.up, out hit);
			
			if(input == new Vector2(0f,0f) && m_IsGrounded && !m_PreviouslyCrouched && GetComponent<Rigidbody>().velocity.sqrMagnitude < 1.5f && !Input.GetButton("Jump") && Vector3.Angle(hit.normal, Vector3.up) < slopeLimit){
				GetComponent<Rigidbody>().isKinematic = true;
			}else{
				GetComponent<Rigidbody>().isKinematic = false;
			}
		}


		private float SlopeMultiplier()
		{
			float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
			return movementSettings.SlopeCurveModifier.Evaluate(angle);
		}


		private void StickToGroundHelper()
		{
			RaycastHit hitInfo;
			if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
				((m_Capsule.height/2f) - m_Capsule.radius) +
				advancedSettings.stickToGroundHelperDistance, ~0, QueryTriggerInteraction.Ignore))
			{
				if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
				{
					m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
				}
			}
		}


		private Vector2 GetInput()
		{

			Vector2 input = new Vector2
			{
				x = Input.GetAxis("Horizontal"),
				y = Input.GetAxis("Vertical")
			};
			movementSettings.UpdateDesiredTargetSpeed(input);
			return input;
		}


		private void RotateView()
		{
			//avoids the mouse looking if the game is effectively paused
			if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

			// get the rotation before it's changed
			float oldYRotation = transform.eulerAngles.y;

			mouseLook.LookRotation (transform, cam.transform);

			if (m_IsGrounded || advancedSettings.airControl)
			{
				// Rotate the rigidbody velocity to match the new direction that the character is looking
				Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
				m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
			}
		}

		/// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
		private void GroundCheck()
		{
			m_PreviouslyGrounded = m_IsGrounded;
			RaycastHit hitInfo;
			if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
				((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, ~0, QueryTriggerInteraction.Ignore))
			{
				m_IsGrounded = true;
				movementSettings.m_IsGrounded = true;
				m_GroundContactNormal = hitInfo.normal;
			}
			else
			{
				m_IsGrounded = false;
				movementSettings.m_IsGrounded = false;
				m_GroundContactNormal = Vector3.up;
			}
			if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
			{
				m_Jumping = false;
			}
		}
		
		//This checks if there is anything above our character and prevents him from jumping/uncrouching to avoid glitching through floors/walls
		public bool NothingAbove(){
			float xMargin = 0.4f;
			float yMargin = 0.5f;
			float distanceToCheck = 0.7f;
			
			return !Physics.Raycast (transform.position + new Vector3 (0, yMargin, 0), Vector3.up, distanceToCheck) && !Physics.Raycast (transform.position + new Vector3 (xMargin, yMargin, 0), Vector3.up, distanceToCheck) && !Physics.Raycast (transform.position + new Vector3 (-xMargin, yMargin, 0), Vector3.up, distanceToCheck) && !Physics.Raycast (transform.position + new Vector3 (0, yMargin, xMargin), Vector3.up, distanceToCheck) && !Physics.Raycast (transform.position + new Vector3 (0, yMargin, -xMargin), Vector3.up, distanceToCheck);
		}
		
		public IEnumerator setPreviouslyCrouched(){
			yield return new WaitForSeconds(0.15f);
			m_PreviouslyCrouched = false;
		}
	}
}
