using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour {
	public MainMenu MainM;
	public Sprite[] sprites;
	public Image slidesboard;
	float resx;
	float resy;
	int i=0;

	void Start() {
		slidesboard.sprite = sprites [i];
		//resy = Screen.height*0.8f;
		//resx = (1920f / 1080f) * resy;
		//RectTransform transform = slidesboard.gameObject.GetComponent<RectTransform> ();
		//transform.sizeDelta = new Vector2(resx,resy);
	}
	public void Next(){
		i += 1;
		if (i == 7) {
			if (GlobalVariables.HighestReachedLevel == -1 && GlobalVariables.CurrentLevel == -1) {
				MainM.Loading.enabled = true;
				i = 0;
				SceneManager.LoadScene ("Level0");
			} else {
				gameObject.GetComponent<Canvas> ().enabled = false;
				i = 0;
				slidesboard.sprite = sprites[i];
				}
			} else {
			slidesboard.sprite = sprites[i];
		}
	}
}
