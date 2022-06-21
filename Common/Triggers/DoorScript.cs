using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
	AudioSource Audio;
	public AudioClip[] DoorSound = new AudioClip[2];
	float dist;
	float maxdist = 3.2f;
	float TimeClosed;
	public IEnumerator Action;
	void Start() {
		Audio = GetComponent<AudioSource> ();
		StopAllCoroutines ();
		//Action = OpenDoor ();
		//StartCoroutine (Action);
	}
	public void Open () {
		Audio.clip = DoorSound [0];
		Audio.Play();
		StopAllCoroutines ();
		Action = OpenDoor ();
		StartCoroutine (Action);
		TimeClosed = 0;
	}
	public void Close () {
		Audio.clip = DoorSound [1];
		Audio.Play();
		StopAllCoroutines ();
		Action = CloseDoor ();
		StartCoroutine (Action);
	}
	void DelayOpen () {
		StopAllCoroutines ();
		Action = DelayOpenDoor ();
		StartCoroutine (Action);
	}
	IEnumerator OpenDoor() {
		while (dist < maxdist) {
			dist = dist + 1.3f*Time.deltaTime;
			transform.Translate (Vector3.up * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}
	IEnumerator CloseDoor() {
		while (dist > 0.01f ) {
			dist = dist - 1.3f*Time.deltaTime;
			transform.Translate (-Vector3.up * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
		DelayOpen ();
	}
	IEnumerator DelayOpenDoor(){
		while (TimeClosed < 600*Time.deltaTime ) {
			TimeClosed += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		Open ();
	}
}
