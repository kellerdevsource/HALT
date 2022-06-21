using UnityEngine;
using System.Collections;

public class ClearPlayerPrefs : MonoBehaviour {

	// Use this for initialization
	void OnAwake () {
		PlayerPrefs.DeleteAll ();
	}
	
	// Update is called once per frame

}
