using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour {
	public DoorScript[] DoorObjectScript = new DoorScript [2];
	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player") {
			foreach (DoorScript i in DoorObjectScript) {
				i.Open ();
			}
		}
	}
	void OnTriggerExit(Collider col) {
		if (col.tag == "Player") {
			foreach (DoorScript i in DoorObjectScript) {
				i.Close ();
			}
		}
	}
}
