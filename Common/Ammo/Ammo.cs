using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {
	public Master M;
	public bool isFlying;
	public bool isDestroyable;
	public MyCharacterController PlayerScript;
	// Use this for initialization
	void Start () {
		if (isDestroyable) {
			Destroy (gameObject, 20f);
		}
	}
	void OnTriggerEnter(Collider Col) {
		if (Col.gameObject.tag == "Player") {
			PlayerScript.AddAmmo();
			Destroy (gameObject, 0);
		}
	}
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 100 * Time.deltaTime, 0);
		if (isFlying) {
			transform.Translate (0, Time.deltaTime / 2, 0);
		}
	}
}
