using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
	public Master M;
	MyCharacterController PlayerScript;
	// Use this for initialization
	void Start () {
	
	}
	void OnTriggerEnter(Collider Col) {
		if (Col.gameObject.tag == "Player") {
			PlayerScript = Col.gameObject.GetComponent<MyCharacterController> ();
			PlayerScript.AddHealth();
			PlayerScript.AddAmmo ();
			Destroy (gameObject, 0);
		}
	}
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 100 * Time.deltaTime, 0);
	}
}
