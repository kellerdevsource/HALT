using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening_Scene_Master : Master {

	void Awake() {
		PlayerPrefs.SetInt ("HighestReachedLevel", -1);
	}
}
