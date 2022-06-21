using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables :MonoBehaviour {
	public static int Score = 0;
	public static int HighScore = 0;
	public static int HighestReachedLevel = -1;
	public static int LevelStage = 0;
	public static int CurrentLevel = -1;
	public static int PlayerCheckPointHealth = 100;
	public static int AmmoChekckPoint = 100;
	public static Vector3 PlayerCheckPointPosition = Vector3.zero;
	public static Vector3 PlayerCheckPointEuler = Vector3.zero;
	public static int privacy;

	void Awake() {
		//PlayerPrefs.DeleteAll ();
		if (SystemInfo.systemMemorySize < 1060 || SystemInfo.processorFrequency < 1500) {
			QualitySettings.masterTextureLimit = 1;
		}
		GetVariables ();
		DontDestroyOnLoad (this.gameObject);
	}
	public static void GetVariables() {
		if (PlayerPrefs.HasKey("privacy")){
			privacy = PlayerPrefs.GetInt ("privacy");
		}
		if (PlayerPrefs.HasKey("Score")){
			Score = PlayerPrefs.GetInt ("Score");
		}
		if (PlayerPrefs.HasKey("HighScore")){
			HighScore = PlayerPrefs.GetInt ("HighScore");
		}
		if (PlayerPrefs.HasKey("HighestReachedLevel")){		
			HighestReachedLevel = PlayerPrefs.GetInt ("HighestReachedLevel");
		}
		if (PlayerPrefs.HasKey("LevelStage")){		
			LevelStage = PlayerPrefs.GetInt ("LevelStage");
		}
		if (PlayerPrefs.HasKey("PlayerCheckPointHealth")){	
			PlayerCheckPointHealth = PlayerPrefs.GetInt ("PlayerCheckPointHealth");
		}
		if (PlayerPrefs.HasKey("AmmoChekckPoint")){	
			AmmoChekckPoint = PlayerPrefs.GetInt ("AmmoChekckPoint");
		}
		if (PlayerPrefs.HasKey("PlayerCheckPointPositionX")){	
			PlayerCheckPointPosition.x = PlayerPrefs.GetFloat("PlayerCheckPointPositionX");
		}
		if (PlayerPrefs.HasKey("PlayerCheckPointPositionY")){	
			PlayerCheckPointPosition.y = PlayerPrefs.GetFloat ("PlayerCheckPointPositionY");
		}
		if (PlayerPrefs.HasKey("PlayerCheckPointPositionZ")){	
			PlayerCheckPointPosition.z =  PlayerPrefs.GetFloat ("PlayerCheckPointPositionZ");
		}
	}
	public static void SetVariables() {
		PlayerPrefs.SetInt ("privacy", privacy);
		PlayerPrefs.SetInt ("Score", Score);
		PlayerPrefs.SetInt ("HighScore", HighScore);
		PlayerPrefs.SetInt ("HighestReachedLevel", HighestReachedLevel);
		PlayerPrefs.SetInt ("LevelStage", LevelStage);
		PlayerPrefs.SetInt ("PlayerCheckPointHealth", PlayerCheckPointHealth);
		PlayerPrefs.SetInt ("AmmoChekckPoint", AmmoChekckPoint);
		PlayerPrefs.SetFloat ("PlayerCheckPointPositionX", PlayerCheckPointPosition.x);
		PlayerPrefs.SetFloat ("PlayerCheckPointPositionY", PlayerCheckPointPosition.y);
		PlayerPrefs.SetFloat ("PlayerCheckPointPositionZ", PlayerCheckPointPosition.z);
		PlayerPrefs.Save ();
	}
}
