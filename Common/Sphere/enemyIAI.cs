using UnityEngine;
using System.Collections;

public class enemyIAI : MonoBehaviour {
	private RaycastHit hit;
	public float RandomMotionTime = 0.5f;
	public float ReturnToPlayerDistance = 7f;
	public int PatrolTime = 1;
	bool Open;
	bool Shooting;
	bool AlreadySpawnedHealth;
	int Possibility;
	Vector3 HitVector;
	Vector3 AIdirection;
	Vector3 AImovement;
	Vector3 AImotion;
	Vector3 AIrotation = Vector3.forward;
	Vector3 BufferVelocityRotate;
	Vector3 BufferVelocityMove;
	Vector3 AIdirectionUnclamped;
	AudioSource[] SphereAudio;
	Animator AnimationControl;
	CharacterController EnemyAI;
	Projector[] Projectors;
	[HideInInspector] public bool dead;
	public bool IsEnemy;
	public float ShootingSpeed;
	public Master M;
	public GameObject Player;
	private GameObject PlayerBuffer;
	public GameObject LaserBullet;
	public Transform LaserShotPosition;
	[SerializeField] private GameObject Patrol = null;
	[SerializeField] private float speed = 3;
	[SerializeField] private int Life = 5;
	MyCharacterController PlayerScript;
	// Use this for initialization
	void Start(){
		Possibility = Random.Range (0, 5);
		PlayerScript = Player.GetComponent<MyCharacterController> ();
		SphereAudio = gameObject.GetComponents<AudioSource>(); 
		AnimationControl = GetComponent<Animator> ();
		EnemyAI = GetComponent<CharacterController> ();
		InvokeRepeating ("PlayerCheck", 1f, 2f);
		Invoke ("DirectedMotion", 0.1f);
		Invoke ("DirectedDirection",0.1f);
		SphereAudio [1].Play ();
		AnimationControl.SetFloat ("Fire",-2f);
		AnimationControl.SetFloat ("Life",-2f);
		PlayerBuffer = Player;
		if (Patrol.gameObject != null) {
			StartCoroutine (PatrolBehaviour ());
		}
	}
	void Update () {
		if (Time.timeScale == 1) {
		float TiltAngle = Vector3.Angle (transform.forward, AIrotation);
		transform.forward = AIrotation;
		transform.Rotate (0, 0, TiltAngle);
		AIrotation = Vector3.SmoothDamp (transform.forward, AIdirection, ref BufferVelocityRotate, 0.2f);
		AImotion = Vector3.SmoothDamp (Vector3.zero, AImovement, ref BufferVelocityMove,0.1f);
		if (Open && Life>=1) {
			if (AnimationControl.GetFloat ("Fire") < 1.9f) {
				AnimationControl.SetFloat ("Fire", 2f, 0.3f, Time.deltaTime);
				Shooting = false;
			} else {
				if (!Shooting) {
					Shooting = true;
					Invoke ("Fire", Time.deltaTime);
				}
			}

		}
		if (!Open && AnimationControl.GetFloat ("Fire") >= -2) {
			AnimationControl.SetFloat ("Fire", -2f, 0.3f, Time.deltaTime);
			Shooting = false;
		}
		if (Life < 1) {
			AnimationControl.SetFloat ("Life", 2f,0.2f,Time.deltaTime);
			EnemyAI.Move (Vector3.up * (-3 * Time.deltaTime));
		} else {	
			EnemyAI.Move (AImotion * speed * Time.deltaTime);
		}
	}
	}
	void OnControllerColliderHit() {
		if (dead && !AlreadySpawnedHealth) {
			if (Possibility == 0) {
				AlreadySpawnedHealth = true;
				Instantiate (M.HealthPrefab, (transform.position), Quaternion.Euler (Vector3.up));
			}
			if (Possibility == 1) {
				AlreadySpawnedHealth = true;
				Instantiate (M.AmmoPrefab, (transform.position), Quaternion.Euler (Vector3.up));
			}
		}
	}
	// Update is called once per frame
	void PlayerCheck() {
		if (Physics.Raycast (transform.position, transform.forward, out hit, 10f)) {
			if (hit.collider.tag == "Player") {
			{
				Open = true;
			}
			} else {
				Open = false;
			}
		}
	}
	void Fire() { 
		if (IsEnemy) {
			if (Open && Shooting) {
				Invoke ("Fire", ShootingSpeed);
			}
			if (Physics.Raycast (transform.position, transform.forward, out hit, 50f)) {
				if (hit.collider.tag == "Player") {
					PlayerScript.ApplyDamage (transform.position);
				}
			}
			HitVector = hit.point - LaserShotPosition.position;
			GameObject RayClone = Instantiate (LaserBullet, LaserShotPosition.position, LaserShotPosition.rotation) as GameObject; 
			Vector3 cloneTransform = RayClone.transform.localScale;
			cloneTransform.z = HitVector.magnitude;
			RayClone.transform.localScale = cloneTransform;
			Destroy (RayClone.gameObject, 0.02f);
			SphereAudio [Random.Range(3,6)].Play ();
		}
	}
	IEnumerator PatrolBehaviour() {
		while (true) {
			if ((PlayerBuffer.transform.position-gameObject.transform.position).magnitude < ReturnToPlayerDistance && Random.value < 0.6f) {
				Player = PlayerBuffer;
			}else {
					Player = Patrol;
				}
			yield return new WaitForSeconds (PatrolTime);
		}
	}
	void DirectedDirection() {
		AIdirectionUnclamped = Player.transform.position - EnemyAI.transform.position;
		AIdirection = AIdirectionUnclamped.normalized;
		if (EnemyAI.velocity.magnitude < 0.5f && !Open) {
			Invoke ("RandomDirection", RandomMotionTime);
		} else {
			Invoke ("DirectedDirection",2*Time.deltaTime);
		}
	}
	void RandomDirection() {
		AIdirection = Random.onUnitSphere;
		Invoke ("DirectedDirection", 1f);
	}
	void RandomMotion () {
		AImovement = Random.onUnitSphere;
		Invoke ("DirectedMotion", Random.Range(0.5f, RandomMotionTime));
		if (AIdirectionUnclamped.magnitude < 3) {
			AImovement.z = 0;
		}
	}
	void DirectedMotion () {
		Invoke ("RandomMotion", Random.Range(1f, 2f));
		if (AIdirectionUnclamped.magnitude < 3) {
			AImovement = -AImovement;
		} else {
			AImovement = Mathf.Clamp(AIdirectionUnclamped.magnitude, 0.5f, 2f)*AIdirection;
		}
	}
	public void ApplyDamage(Vector3 HitNormal) {
		Life -= 1;
		AIdirection = HitNormal;
		AImovement = -HitNormal;
		CancelInvoke ("DirectedDirection");
		SphereAudio [2].Play ();
		if (!dead && Life == 0) {
			dead = true;
			CancelInvoke ();
			SphereAudio [1].Stop ();
			SphereAudio [0].Play ();
			EnemyAI.radius = 0.35f;
			Destroy (gameObject, 20f);
			M.OnEnemyKill();
			Projectors = GetComponentsInChildren<Projector> ();
			Projectors [0].enabled = true;
			Projectors [1].enabled = false;
			SphereShadowRotation PositionScript = Projectors [0].GetComponent<SphereShadowRotation> ();
			PositionScript.enabled = true;
		} else if (Life > 0) {
			Invoke ("DirectedDirection", 0.5f);
		}
	}
	public void HeadShot(Vector3 HitNormal) {
		if (!dead) {
			Life = 0;
			dead = true;
			SphereAudio [1].Stop ();
			SphereAudio [2].Play ();
			AIdirection = -HitNormal;
			CancelInvoke ();
			SphereAudio [0].Play ();
			EnemyAI.radius = 0.35f;
			Destroy (gameObject, 10f);
			M.OnEnemyKill ();
			Projectors = GetComponentsInChildren<Projector> ();
			Projectors [0].enabled = true;
			Projectors [1].enabled = false;
			SphereShadowRotation PositionScript = Projectors [0].GetComponent<SphereShadowRotation> ();
			PositionScript.enabled = true;
		}
	}
}
