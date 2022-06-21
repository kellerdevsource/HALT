using UnityEngine;
using System.Collections;

public class MyCharacterController : MonoBehaviour {
	Vector3 Velocity;
	Vector3 CamRot;
	Vector3 EnemyPosition;
	AudioSource[] PlayerAudio;
	int i;
	public LaserShot Weapon;
	public Transform CameraT;
	private AudioListener Listener;
	public TouchInputs TI;
	public Master M;
	public int Health;
	public bool locked;
	float CamRotationX;
	float CamRotationZ;
	float CameraSmoothingVelocityX;
	float CameraSmoothingVelocityZ;
	Vector3 CameraVectorSmoothBuffer;
	Vector3 EnemyPositionBuffer;
	float tiltVelocity;
	float VelocityY;
	CharacterController chc;
	[SerializeField] private float CameraViewAngleUp = 60;
	[SerializeField] private float CameraViewAngleTilt = 10;
	[SerializeField] private float CameraSmoothing = 0.2f;
	[SerializeField] private float RoSpeed = 100;
	[SerializeField] private float walkSpeed = 5;
	[SerializeField] private float gravity = -50;
	[SerializeField] private float jumpHeight = 1;
	[HideInInspector] public bool dead;
	[HideInInspector] public bool move;
	bool footsteps;
	// Use this for initialization
	void Start () {
		chc = GetComponent<CharacterController> ();
		PlayerAudio = GetComponents<AudioSource> ();
	}
	// Update is called once per frame
	void Update () {
		if (!dead && Time.timeScale == 1) {
			CamRotationX = Mathf.SmoothDamp (CamRotationX, -TI.yrotate * CameraViewAngleUp + EnemyPosition.y * 4, ref CameraSmoothingVelocityX, CameraSmoothing);
			CamRotationZ = Mathf.SmoothDamp (CamRotationZ, -(Input.acceleration.x) * CameraViewAngleTilt + EnemyPosition.x, ref CameraSmoothingVelocityZ, CameraSmoothing);
			CamRot = CameraT.transform.localEulerAngles;
			CamRot.x = CamRotationX;
			CamRot.y = transform.eulerAngles.y;
			CamRot.z = CamRotationZ;
			CameraT.transform.eulerAngles = CamRot;
			transform.Rotate (0, TI.xrotate * RoSpeed * Time.deltaTime, 0);
			VelocityY = VelocityY + gravity * Time.deltaTime;
			Velocity = Vector3.forward * TI.ymove * walkSpeed + Vector3.right * Mathf.Clamp (TI.xmove, -1, 1) * walkSpeed + Vector3.up * VelocityY - EnemyPosition;
			if (move) {
				PlayerAudio [2].volume = 0.6f;
				if (!footsteps && chc.isGrounded && (Mathf.Abs(TI.xmove)>0.3f || Mathf.Abs(TI.ymove) > 0.3f)) { 
					StartCoroutine (FootSteps ());
					footsteps = true;
				}
			} 
			if (!move && footsteps) {
				PlayerAudio [2].volume = 0.2f;
			}
			if (!locked) {
				chc.Move (transform.TransformDirection (Velocity) * Time.deltaTime);
			}
		}
		if (chc.isGrounded) {
			VelocityY = 0;
		}
		if (EnemyPosition.magnitude > 0) {
			EnemyPosition = Vector3.SmoothDamp (EnemyPosition, Vector3.zero, ref EnemyPositionBuffer, 0.05f);
		}
	}
	IEnumerator FootSteps() {
		while (move && (Mathf.Abs(TI.xmove)>0.3f || Mathf.Abs(TI.ymove) > 0.3f)) {
			if (chc.isGrounded) {
				PlayerAudio [2].Play ();
				yield return new WaitForSeconds (0.62f);
			} else {
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}
		footsteps = false;
	}
	public void Jump () {
		PlayerAudio [2].Stop ();
		PlayerAudio [3].Play ();
		if (chc.isGrounded) {
			float jumpSpeed = Mathf.Sqrt (-2 * gravity * jumpHeight);
			VelocityY = jumpSpeed;
		}
	}
	public void ApplyDamage(Vector3 Enemy) {
		if (Health > 0) {
			Health -= 1;
			PlayerAudio [0].Play ();
			EnemyPosition = transform.InverseTransformPoint (Enemy);
			TI.HitDirection (EnemyPosition);
		}
		if (Health <= 0 && dead == false) {
			M.OnPlayerKill ();
			dead = true;
		}
	}
	public void AddHealth() {
		PlayerAudio [1].Play ();
		if (Health >= 90) {
			Health = 100;
			TI.HealthBar();
		}
		if (Health < 90) {
			Health += 10;
			TI.HealthBar();
		} 
	}
	public void AddAmmo() {
		PlayerAudio [1].Play ();
		Weapon.AddAmmo ();
	}

}
