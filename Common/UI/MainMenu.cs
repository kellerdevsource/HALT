using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;   

public class MainMenu : MonoBehaviour {
	private AudioSource UIAudio;
	public AudioMaster AudioM;
	public Text Highscore;
	public Canvas Menu;
	public Canvas SureToStart;
	public Canvas SureToDirectLoad;
	public Canvas SureToQuit;
	public Canvas controls;
	Controls Controlsscript;
	public TouchInputs TInput;
	public Button[] DirectLoad = new Button[4];
	public Button ContinueButton;
	public bool isPlaying = false;
	public Canvas FinalMenu;
	public Canvas Privacy;
	public Canvas Loading;
	Coroutine AdCoroutine;
	int index=0;
	string SceneToLoad;
	// Use this for initialization
	void Start() {

		//GlobalVariables.HighestReachedLevel = 1;

		if (GlobalVariables.privacy == 0 && GlobalVariables.CurrentLevel == -1) {
			Privacy.enabled = true;
		}
		Controlsscript = controls.gameObject.GetComponent<Controls> ();
		Controlsscript.MainM = gameObject.GetComponent<MainMenu> ();
		UIAudio = gameObject.GetComponent<AudioSource> ();
		SureToStart.enabled = false;
		SureToQuit.enabled = false;
		SureToDirectLoad.enabled = false;
		RefreshButtons ();
	}
	public void RefreshButtons() {
		if (GlobalVariables.Score > GlobalVariables.HighScore) {
			GlobalVariables.HighScore = GlobalVariables.Score;
		}
		Highscore.text = "HIGH SCORE: " + GlobalVariables.HighScore;
		for (int i = 0; i < DirectLoad.Length; i++) {
			if (GlobalVariables.HighestReachedLevel < i) {
				DirectLoad [i].interactable = false;
			} else {
				DirectLoad [i].interactable = true;
			}
		}
		if (GlobalVariables.HighestReachedLevel == -1) {
			ContinueButton.interactable = false;
		} else {
			ContinueButton.interactable = true;
		}
		GlobalVariables.SetVariables ();
	}
	public void StartButton() {
		UIAudio.Play();
		index = 0;
		if (GlobalVariables.HighestReachedLevel != -1) {
			SureToStart.enabled = true;
			Menu.enabled = false;
		} else {
			Controls ();
		}
	}
	public void DirectLoadLevel0 (){
		UIAudio.Play ();
		index = 0;
		SureToDirectLoad.enabled = true;
		Menu.enabled = false;
	}
	public void DirectLoadLevel1 (){
		UIAudio.Play ();
		index = 1;
		SureToDirectLoad.enabled = true;
		Menu.enabled = false;
	}
	public void DirectLoadLevel2 (){
		UIAudio.Play ();
		index = 2;
		SureToDirectLoad.enabled = true;
		Menu.enabled = false;
	}
	public void DirectLoadLevel3 (){
		UIAudio.Play ();
		index = 3;
		SureToDirectLoad.enabled = true;
		Menu.enabled = false;
	}
	public void SureToDirectLoadYes() {
		UIAudio.Play ();
		GlobalVariables.LevelStage = 0;
		GlobalVariables.Score = 0;
		GlobalVariables.SetVariables();
		LoadScene ();
		SureToDirectLoad.enabled = false;
		Menu.enabled = false;
	}
	public void SureToDirectLoadNo() {
		#if UNITY_ADS
		if (Advertisement.IsReady ()) {
			ShowAdReturn ();
		} else {
			ActuallyReturn ();
		}
		#else
		ActuallyReturn ();
		#endif
	}
	public void SureToStartYes() {
		UIAudio.Play ();
		GlobalVariables.LevelStage = 0;
		GlobalVariables.Score = 0;
		GlobalVariables.SetVariables();
		LoadScene ();
		SureToStart.enabled = false;
		Menu.enabled = false;
	}
	public void SureToStartNo() {
		#if UNITY_ADS
		if (Advertisement.IsReady ()) {
			ShowAdReturn ();
		} else {
			ActuallyReturn ();
		}
		#else
		ActuallyReturn ();
		#endif
	}
	public void Continue() {
		UIAudio.Play ();
		#if UNITY_ADS
		if (GlobalVariables.HighestReachedLevel > 0 && GlobalVariables.LevelStage > 3 && Advertisement.IsReady ()) {
			AudioListener.volume = 0f;
			var options = new ShowOptions { resultCallback = HandleShowResultContinue };
			Advertisement.Show (options);
		} else {
			ActuallyContinue ();
		}
		#else
		ActuallyContinue ();
		#endif
	}
	public void Quit(){
		UIAudio.Play ();
		Menu.enabled = false;
		SureToQuit.enabled = true;
	}
	public void QuitYes(){
		UIAudio.Play ();
		GlobalVariables.SetVariables ();
		Application.Quit ();
	}
	public void QuitNo(){
		ActuallyReturn ();
	}
	public void Controls(){
		controls.enabled = true;
	}
	public void Rate(){
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.kellerdev.HALT");
	}
	public void FinalMenuActivate(){
		Time.timeScale = 0;
		FinalMenu.enabled = true;
		TouchInputs.paused = true;
		AudioM.Pause();
	}
	public void FinalReturn(){
		Time.timeScale = 1;
		FinalMenu.enabled = false;
		TouchInputs.paused = false;
		AudioM.UnPause();
	}
	void LoadScene(){
		SceneToLoad = "Level" + index;
		Loading.enabled = true;
		SceneManager.LoadScene (SceneToLoad);
		RefreshButtons ();
		Menu.enabled = false;
		AudioListener.pause = false;
		Time.timeScale = 1;
	}
	#if UNITY_ADS
	void HandleShowResultReturn (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			ActuallyReturn ();
			break;
		case ShowResult.Skipped:
			ActuallyReturn ();
			break;
		case ShowResult.Failed:
			ActuallyReturn ();
			break;
		}
	}
	void HandleShowResultContinue (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			ActuallyContinue ();
			break;
		case ShowResult.Skipped:
			ActuallyContinue ();
			break;
		case ShowResult.Failed:
			ActuallyContinue ();
			break;
		}
	}
	#endif
	void ActuallyReturn(){
		RefreshButtons ();
		SureToDirectLoad.enabled = false;
		SureToStart.enabled = false;
		SureToQuit.enabled = false;
		Menu.enabled = true;
		AudioListener.volume = 1f;
	}
	void ActuallyContinue(){
		if (isPlaying) {
			TInput.Resume ();
		} else {
			index = GlobalVariables.HighestReachedLevel;
			LoadScene ();
			Menu.enabled = false;
			AudioListener.volume = 1f;
		}
	}
	void ShowAdReturn(){
		#if UNITY_ADS
		AudioListener.volume = 0f;
		var options = new ShowOptions { resultCallback = HandleShowResultReturn };
		Advertisement.Show (options);
		#endif
	}
	public void PrivacyPolycyOk () {
		GlobalVariables.privacy = 1;
		GlobalVariables.SetVariables ();
		Privacy.enabled = false;
	}
	public void NotOk() {
		Application.Quit ();
	}
	public void UnityPolicy(){
		Application.OpenURL ("https://unity3d.com/de/legal/privacy-policy");
	}
}
