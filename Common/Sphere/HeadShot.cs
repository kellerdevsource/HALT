using UnityEngine;
using System.Collections;

public class HeadShot : MonoBehaviour {
	public void Head(Vector3 HitNormal) {
		GetComponentInParent<enemyIAI> ().HeadShot (HitNormal);
	}
}
