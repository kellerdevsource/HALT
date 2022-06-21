#if(UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;

public class Recorder : MonoBehaviour {
	public string folder = "OutputFolder";
	public int frameRate = 30;
	public bool update;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		//Time.captureFramerate = frameRate;
	}
	
	// Update is called once per frame
	void Update () {
		if (update) {
			string name = string.Format ("{0}/{1:D04} OUTPUT.png", folder, Time.frameCount);
			ScreenCapture.CaptureScreenshot (name);
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			print ("press");
			string name = string.Format ("{0}/{1:D04} OUTPUT.png", folder, Time.frameCount);
			ScreenCapture.CaptureScreenshot (name);
		}
	}
	public void ScreenGrab() {
		print ("press");
		string name = string.Format ("{0}/{1:D04} OUTPUT.png", folder, Time.frameCount);
		ScreenCapture.CaptureScreenshot (name);
	}
}
#endif