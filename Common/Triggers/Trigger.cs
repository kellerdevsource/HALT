using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	public float halftime;
	Color TheColor;
	bool found =false;
	Renderer R;
	Collider col;
	public Master M;
	public bool bomb = false;
	public int ID;
	public int OccupiedLocation;
	AudioSource[] Audio;
	public AudioClip[] checkpointsounds;
	float ColorScale;
	float TimeStart;
	void Start() {
		//gameObject.GetComponentInChildren<Renderer> ().material.SetColor ("_TintColor", TheColor);
		R = GetComponentInChildren<MeshRenderer> ();
		col = GetComponent<BoxCollider> ();
		Audio = GetComponents<AudioSource> ();
		if (bomb) {
			TimeStart = Time.time;
			StartCoroutine (Bomb());
			Audio[1].clip = checkpointsounds[0];
			Audio[1].Play ();
		}
	}
	void OnTriggerEnter(Collider Col) {
		if (Col.gameObject.tag == "Player" && found == false) {
			R.enabled = false;
			col.enabled = false;
			found = true;
			if (bomb) {
				StopAllCoroutines ();
				Audio [0].Stop ();
				Audio[1].clip = checkpointsounds[1];
				Audio[1].Play ();
				M.TimerTimes [ID] = Mathf.Infinity;
				M.BombSpawnLocations [OccupiedLocation] = false;
			}
			M.OnTriggerReach();
			Destroy (gameObject, 2.5f);
		}
	}
	void Update() {
		if (bomb) {
			ColorScale = ((Time.time - TimeStart) / (halftime*2));
			//TheColor.r += ((1-TheColorBegin.r)/halftime)*Time.deltaTime;
			//TheColor.g -= (TheColorBegin.g/halftime)*Time.deltaTime;
			gameObject.GetComponentInChildren<Renderer> ().material.SetColor ("_TintColor", Color.HSVToRGB((1-ColorScale)/4, 1, 1));
		}
		transform.Rotate (0, 100 * Time.deltaTime, 0);
	}
	IEnumerator Bomb() {
		yield return new WaitForSeconds (halftime*1.33f);
		Audio[0].Play ();
		yield return new WaitForSeconds (halftime*0.67f);
		if (found == false) {
			M.OnPlayerKill ();
			Destroy (gameObject,0.0f);
		}
	}
}
