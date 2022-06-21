using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour {
	public MyCharacterController Player;
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
		}
	}
}
