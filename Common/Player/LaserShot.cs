using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {
	[HideInInspector] public int EnemyHit;
	[HideInInspector] public int AmmoUsed;
	public int Ammo;
	//private bool fire;
	public Transform LaserShotPosition;
	public GameObject LaserBullet;
	public GameObject RayImpact;
	public Transform CameraT;
	public TouchInputs TI;
	public Master M;
	Vector3 SmoothingBuffer;
	Vector3 HitVector;
	Vector3 ForwardBuffer;
	AudioSource[] LaserSound;
	Animator AnimationControl;
	// Use this for initialization
	void Start () {
	LaserSound = GetComponents<AudioSource> ();
	AnimationControl = GetComponent<Animator> ();
	TI.Ammo.text = (Ammo - AmmoUsed).ToString();
	}
	public void StartFire(Vector2 TouchPosition) {
		if (Ammo > 0) {
			AmmoUsed += 1;
			Ammo -= 1;
			TI.AmmoUpdate();
			AnimationControl.Play ("Fire", -1, 0f);
			Ray ray;
			RaycastHit hit;
			ray = Camera.main.ScreenPointToRay (TouchPosition);
			if (Physics.SphereCast (ray, 0.05f, out hit)) {
				LaserSound [0].Play ();
				HitVector = hit.point - LaserShotPosition.transform.position;
				GameObject RayClone = Instantiate (LaserBullet, LaserShotPosition.position, Quaternion.LookRotation (HitVector)) as GameObject; 
				Vector3 cloneTransform = RayClone.transform.localScale;
				cloneTransform.z = HitVector.magnitude;
				RayClone.transform.localScale = cloneTransform;
				Destroy (RayClone.gameObject, 0.05f);
				GameObject ImpactClone = (GameObject)Instantiate (RayImpact, hit.point + 0.08f * hit.normal, Quaternion.LookRotation (hit.normal));
				if (hit.collider.gameObject.transform.parent != null) {
					ImpactClone.transform.parent = hit.collider.gameObject.transform.parent.transform;
				} else {
					ImpactClone.transform.parent = hit.collider.gameObject.transform;
				}
				ImpactClone.gameObject.transform.Rotate (0, 0, Random.Range (-180, 180));
				ImpactTransform ImpactTransformClone = ImpactClone.gameObject.GetComponent<ImpactTransform> ();
				ImpactTransformClone.BlowUpInit ();
				Destroy (ImpactClone, 0.5f);
				if (hit.collider.tag == "Enemy") {
					EnemyHit += 1;
					enemyIAI SendDamageToObject = hit.collider.gameObject.GetComponent<enemyIAI> ();
					SendDamageToObject.ApplyDamage (hit.normal);
				}
				if (hit.collider.tag == "EnemyHead") {
					HeadShot SendDamageToObject = hit.collider.gameObject.GetComponent<HeadShot> ();
					if (!SendDamageToObject.GetComponentInParent<enemyIAI> ().dead) {
						EnemyHit += 1;
						SendDamageToObject.Head (hit.normal);
						LaserSound [1].Play ();
					}
				}
			} else {
				LaserSound [0].Play ();
				GameObject RayClone = Instantiate (LaserBullet, LaserShotPosition.position, Quaternion.LookRotation (transform.forward)) as GameObject; 
				Vector3 cloneTransform = RayClone.transform.localScale;
				cloneTransform.z = 50;
				RayClone.transform.localScale = cloneTransform;
				Destroy (RayClone.gameObject, 0.05f);
			}
		} else {
			LaserSound [2].Play ();
			M.OnAmmoEmpty();
	}
	// Update is called once per frame
	
	}
	public void AddAmmo(){
		Ammo += 20;
		TI.Ammo.text = Ammo.ToString();
	}
			
}
