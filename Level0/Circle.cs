using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {
	public MyCharacterController Player;
	public int speed = 55;
	bool update;
	AudioSource Audio;
	void Start(){
		Audio = GetComponent<AudioSource> ();
	}
	void Update () {
		if (update) {
			transform.Rotate (0, 0, Time.deltaTime * 2);
			if (transform.localScale.z < 2.5f) {
				transform.localScale = new Vector3 (1.2f, 1.2f, transform.localScale.z + Time.deltaTime / speed);
			}
		}
	}
	public void moove(){
		update = true;
		Audio.Play ();
	}
	/*void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			Player.die ();
		}
	}
	public void redo(Vector3 Scale){
		transform.localScale = new Vector3 (Scale.x, Scale.y, Scale.z);
	}
	*/
}
