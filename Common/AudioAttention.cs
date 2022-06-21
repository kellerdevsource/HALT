using UnityEngine;
using System.Collections;

public class AudioAttention : MonoBehaviour {
	AudioSource Attention;
	// Use this for initialization
	void Start () {
		Attention = GetComponent<AudioSource>();
		StartCoroutine(PlayAttention());
	}
	
	// Update is called once per frame
	IEnumerator PlayAttention () {
		yield return new WaitForSeconds (10);
			while(true) {
				Attention.Play ();
				yield return new WaitForSeconds (20);
			}
	}
}
