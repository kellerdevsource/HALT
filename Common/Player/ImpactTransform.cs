using UnityEngine;
using System.Collections;

public class ImpactTransform : MonoBehaviour {
	bool Blow;
	// Use this for initialization
	void Start () {
	
	}

	public void BlowUpInit(){
		StartCoroutine (BlowUp());
	}
	IEnumerator BlowUp() {
		Blow = true;
		while (Blow) {
			Vector3 BlowUp = transform.localScale;
			BlowUp.x += 2 * Time.deltaTime;
			BlowUp.y += 2 * Time.deltaTime;
			yield return new WaitForSeconds (0.01f);
			transform.localScale = BlowUp;
			if (transform.localScale.magnitude > 0.5f) {
				Blow = false;
			}
		}
		while (!Blow) {
			Vector3 BlowUp = transform.localScale;
			BlowUp.x -= 2 * Time.deltaTime;
			BlowUp.y -= 2 * Time.deltaTime;
			yield return new WaitForSeconds (0.01f);
			transform.localScale = BlowUp;
			if (transform.localScale.magnitude < 0.3f) {
				Blow = true;
			}
		}
	}

}
