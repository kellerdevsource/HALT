using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut : MonoBehaviour {
	public bool FadeIn;
	public Canvas BlackCanvas;
	CanvasGroup Canvas;
	void Start () {
		Canvas = GetComponent<CanvasGroup> ();
		if (FadeIn) {
			StartCoroutine (In ());
		} 
	}
	IEnumerator In(){
		Canvas.alpha = 1;
		while (Canvas.alpha >= 0) {
			Canvas.alpha -= 0.1f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}
	IEnumerator Out(){
		Canvas.alpha = 0;
		while (Canvas.alpha < 1) {
			Canvas.alpha += 0.1f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}
}
