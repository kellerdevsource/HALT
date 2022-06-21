using UnityEngine;
using System.Collections;

public class k : MonoBehaviour {
	bool Open;
	bool Shooting;
	AudioSource[] SphereAudioControl;
	Animator AnimationControl;
	CharacterController EnemyAI;
	Projector[] RingLaser;
	public Master M;
	public GameObject Player;
	public float speed = 3;
	public float Life = 100;
	Vector3 AIdirection;
	Vector3 AImovement;
	Vector3 AIrotation;
	Vector3 BufferVelocityRotate;
	Vector3 BufferVelocityMove;
	Vector3 AIdirectionUnclamped;
	// Use this for initialization
	void Start(){
		SphereAudioControl = gameObject.GetComponents<AudioSource>(); 
		AnimationControl = GetComponent<Animator> ();
		EnemyAI = GetComponent<CharacterController> ();
		InvokeRepeating ("PlayerCheck", 1f, 1f);
		Invoke ("DirectedMotion", 0.1f);
		Invoke ("DirectedDirection",0.1f);
		SphereAudioControl [1].Play ();
		gameObject.SetActive (true);
		AnimationControl.SetFloat ("Fire",-2f);
		AnimationControl.SetFloat ("Life",-2f);
	}
	// Update is called once per frame
	void Update () {
		float TiltAngle = Vector3.Angle (transform.forward, AIdirection);
		transform.forward = AIrotation;
		transform.Rotate (0, 0, TiltAngle);
		AIrotation = Vector3.SmoothDamp (transform.forward, AIdirection, ref BufferVelocityRotate, 1.9f);
		Debug.Log (Open);
		if (Open && Life>1) {
			if (AnimationControl.GetFloat ("Fire") < 1.5f) {
				AnimationControl.SetFloat ("Fire", 2f, 0.3f, Time.deltaTime);
			} else {
				if (!Shooting) {
					Shooting = true;
					Fire ();
				}
			}

		}
		if (!Open && AnimationControl.GetFloat ("Fire") > -2) {
			AnimationControl.SetFloat ("Fire", -2f, 0.3f, Time.deltaTime);
			Shooting = false;
		}
		if (Life < 1) {
			AnimationControl.SetFloat ("Life", 2f,0.2f,Time.deltaTime);
			EnemyAI.Move (Vector3.up * (-3 * Time.deltaTime));
		} else {
			EnemyAI.Move (AImovement * speed * Time.deltaTime);
		}
	}
	void PlayerCheck() {
		RaycastHit PCheck;
		Physics.Raycast (transform.position, transform.forward, out PCheck, 10f);
		if (PCheck.collider.gameObject!=null && PCheck.collider.tag == "Player") {
			Open = true;
		} else {
			Open = false;
		}
	}
	void Fire() { 
		if (Shooting){
			Invoke ("Fire", 0.5f);
		}
	}
	void DirectedDirection() {
		Debug.Log ("directed");
		AIdirectionUnclamped = Player.transform.position - EnemyAI.transform.position;
		AIdirection = AIdirectionUnclamped.normalized;
		if (EnemyAI.velocity.magnitude < 0.3f) {
			Invoke ("RandomDirection", 0.0f);
		} else {
			Invoke ("DirectedDirection",0.0f);
		}
	}
	void RandomDirection() {
		Debug.Log ("random");
		AIdirection = Random.onUnitSphere;
		Invoke ("DirectedDirection",0.5f);
	}
	void RandomMotion () {
		AImovement = (Random.onUnitSphere + AImovement)/2;

		Invoke ("DirectedMotion", 0.5f);
		if (AIdirectionUnclamped.magnitude < 3) {
			AImovement.z = 0;
		}
	}
	void DirectedMotion () {

		AImovement = Vector3.SmoothDamp (transform.forward, AIdirection, ref BufferVelocityMove, 2f);
		Invoke ("RandomMotion", 1f);
		if (AIdirectionUnclamped.magnitude < 3) {
			AImovement = -AImovement;
		}
	}
	public void ApplyDamage(Vector3 HitNormal) {
		AIdirection = -HitNormal;
		CancelInvoke ("DirectedDirection");
		Life -= 1;
		if (Life == 0) {
			CancelInvoke ();
			SphereAudioControl [1].Stop ();
			SphereAudioControl [0].Play ();
			Destroy (gameObject, 10f);
			M.OnEnemyKill();
			RingLaser = GetComponentsInChildren<Projector> ();
			foreach (Projector P in RingLaser) {
				P.enabled = false;
			}
			RingLaser [0].enabled = true;
			SphereShadowRotation PositionScript = RingLaser [0].GetComponent<SphereShadowRotation> ();
			PositionScript.enabled = true;
		} else if (Life > 0) {
			Invoke ("DirectedDirection", 0.5f);
		}
	}
}
