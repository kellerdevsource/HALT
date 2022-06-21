using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchInputs : MonoBehaviour {
	Vector2 TouchPos;
	Vector2[] BeginTouchPos = new Vector2[3];
	public Text score;
	public GameObject Player;
	private MyCharacterController Character;
	public LaserShot Weapon;
	public Button MoveButton;
	private RectTransform MoveButtonTransform;
	public Button RotateButton;
	public Button JumpButton;
	public Button FireButton;
	public Text Ammo;
	public Image Up;
	public Image Back;
	public Image Right;
	public Image Left;
	public Image Health;
	public Image Enemy;
	public GameObject Accuracy;
	public Image Kill;
	public GameObject MainUiObject;
	private CanvasGroup KillBarAlpha;
	public Canvas Status;
	private MainMenu MainUIscript;
	private Canvas MainUI;
	public AudioMaster AudioM;
	public Master LevelMaster;
	public Image TimerBar;
	public Image MoveAnchor;
	public Image RotateAnchor;
	public Image MoveCircle;
	public Image RotateCircle;
	IEnumerator TimerActive;
	bool TimerBarActive;
	float TimerActiveTime;
	float TimerBarScale;
	Text[] AccuracyText;
	int m;
	int r;
	int i;
	int ShotAccuracyOperator;
	float ShotAccuracy;
	float HealthBarScale;
	float EnemyBarScale;
	float scale;
	[HideInInspector] public static bool paused;
	float Resx;
	float Resy;
	public  float ymove;
	public  float xmove;
	public  float yrotate;
	public  float xrotate;
	Vector2 FireTouchPos;
	void OnAwake () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	// Use this for initialization
	void Start () {
		Resx = Screen.width;
		Resy = Screen.height;
		FireTouchPos = new Vector2(Resx/2,Resy/2);
		Character = Player.GetComponent<MyCharacterController> ();
		AccuracyText = Accuracy.GetComponentsInChildren<Text>();
		HealthBarScale = 0.355f*1920;
		EnemyBarScale =  0.355f*1920;
		KillBarAlpha = Kill.GetComponent<CanvasGroup>();
		MainUI = MainUiObject.GetComponent<Canvas> ();
		MainUIscript = MainUiObject.GetComponent<MainMenu> ();
		TimerActive = Timer ();
	}

	public void StopRotate() {
		xrotate = 0;
		RotateAnchor.enabled = false;
		RotateCircle.enabled = false;
	}
	public void StopMove() {
		ymove = 0;
		xmove = 0;
		Character.move = false;
		MoveAnchor.enabled = false;
		MoveCircle.enabled = false;
	}
	void findtouchid() {
			for (i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch (i).position.x < Resx / 2) {
					m = i;
				}
				if (Input.GetTouch (i).position.x > Resx / 2) {
					r = i;
				}
			}
	}
	public void InitiateMove(){
		findtouchid ();
		if (Weapon.isActiveAndEnabled) {
			Weapon.StartFire (FireTouchPos);
		}
		BeginTouchPos[0] = Input.GetTouch (m).position;
		MoveAnchor.rectTransform.anchoredPosition = BeginTouchPos [0]*(1920/Resx);
		MoveCircle.rectTransform.anchoredPosition = BeginTouchPos [0]*(1920/Resx);
		MoveAnchor.enabled = true;
		MoveCircle.enabled = true;
	}
	public void InitiateRotate(){
		findtouchid ();
		BeginTouchPos[1] = Input.GetTouch (r).position;
		RotateAnchor.rectTransform.anchoredPosition = BeginTouchPos [1]*(1920/Resx);
		RotateCircle.rectTransform.anchoredPosition = BeginTouchPos [1]*(1920/Resx);
		RotateAnchor.enabled = true;
		RotateCircle.enabled = true;
	}
	public void Rotate(){
		findtouchid ();
		yrotate = (Input.GetTouch (r).position.y - BeginTouchPos [1].y)*(1920/Resx);
		yrotate = Mathf.Clamp (yrotate * 0.005f, -1, 1);
		xrotate = (Input.GetTouch (r).position.x - BeginTouchPos [1].x)*(1920/Resx);
		xrotate = Mathf.Clamp (xrotate * 0.005f, -1, 1);
		RotateCircle.rectTransform.anchoredPosition = Input.GetTouch (r).position*(1920/Resx);
	}
	public void Move(){
		findtouchid ();
		ymove = (Input.GetTouch (m).position.y - BeginTouchPos [0].y)*(1920/Resx);
		ymove = Mathf.Clamp (ymove * 0.005f, -1, 1);
		xmove = (Input.GetTouch (m).position.x - BeginTouchPos [0].x)*(1920/Resx);
		xmove = Mathf.Clamp (xmove * 0.005f, -1, 1);
		MoveCircle.rectTransform.anchoredPosition = Input.GetTouch (m).position*(1920/Resx);
		Character.move = true;
	}
	/*
	public void StartBevaviour() {
		ymove = 0;
		xmove = 0;
		xrotate = 0;
		for (i = 0; i < Input.touchCount; ++i) {
			TouchPos = Input.GetTouch (i).position;
			if (TouchPos.x < ButtonSize && TouchPos.y < ButtonSize) {
				ymove = (TouchPos.y/ButtonSize)*2 - 1;
				ymove = Mathf.Clamp (ymove * 1.2f, -1, 1);
				xmove = (TouchPos.x/ButtonSize)*2 - 1;
				xmove = Mathf.Clamp (xmove * 1.2f, -1, 1);
				Character.move = true;
			} 
			if (TouchPos.x > (Resx-ButtonSize) && TouchPos.y < ButtonSize) {
				yrotate = (TouchPos.y /ButtonSize)*2 - 1;
				yrotate = Mathf.Clamp (yrotate * 1.2f, -1, 1);
				xrotate =  2*((TouchPos.x - Resx)/ButtonSize)+1;
				xrotate = Mathf.Clamp (xrotate * 1.2f, -1, 1);
			}
		}
	}
	public void StopMotion() {
		ymove = 0;
		xmove = 0;
		Character.move = false;
	}
	public void StopRotation() {
		xrotate = 0;
	}
	*/
	public void Jump() {
		Character.Jump ();
	}
	public void Fire() {
		if (Weapon.isActiveAndEnabled) {
			Weapon.StartFire (FireTouchPos);
		}
	}
	public void AmmoUpdate() {
		Ammo.text = Weapon.Ammo.ToString();
	}
	public void HitDirection(Vector3 EnemyPosition) {
		HealthBar ();
		if (EnemyPosition.x > 0) {
			Right.GetComponent<Image> ().enabled = true;
		}
		if (EnemyPosition.x < 0) {
			Left.GetComponent<Image> ().enabled = true;
		}
		if (EnemyPosition.z < 0) {
			Back.GetComponent<Image> ().enabled = true;
		}
		if (EnemyPosition.y < 0) {
			Up.GetComponent<Image> ().enabled = true;
		}
		EnemyPosition = Vector3.zero;
		Invoke ("NoHit", 0.2f);
	}
	void NoHit() {
		Right.GetComponent<Image> ().enabled = false;
		Left.GetComponent<Image> ().enabled = false;
		Back.GetComponent<Image> ().enabled = false;
		Up.GetComponent<Image> ().enabled = false;
	}
	public void HealthBar() {
		float H = Character.Health;
		Health.rectTransform.offsetMax = new Vector2(-(1-(H/100))*HealthBarScale,0);
	}
	public void EnemyBar() {
		score.text = "SCORE: " + GlobalVariables.Score;
		if (LevelMaster.TotalNumberOfEnemy > 0) {
			float T = LevelMaster.TotalNumberOfEnemy;
			float E = LevelMaster.EnemyKilled;
			Enemy.rectTransform.offsetMax = new Vector2 (( - (E/T)*EnemyBarScale), 0);
			if (E == T && E != 0) {
				ShotAccuracy = ((Weapon.EnemyHit * 1.0f) / (Weapon.AmmoUsed * 1.0f)) * 100;
				StartCoroutine (ShotAccuracyCounter ());
			}
		} else {
			Enemy.rectTransform.offsetMax = new Vector2(0,0);
		}
	}
	public void Menu() {
		Time.timeScale = 0;
		paused = true;
		MainUI.enabled = true;
		Status.enabled = false;
		StopMove ();
		StopRotate ();
		gameObject.GetComponent<Canvas> ().enabled = false;
		AudioM.Pause();
		MainUIscript.RefreshButtons ();
	}
	public void Resume() {
		Time.timeScale = 1;
		MainUI.enabled = false;
		Status.enabled = true;
		gameObject.GetComponent<Canvas> ().enabled = true;
		AudioM.UnPause();
		paused = false;
		AudioListener.volume = 1f;
	}
	public void PlayerKill(){
		Kill.enabled = true;
		StartCoroutine (KillBar ());
	}
	public void PlayerRevive(){
		KillBarAlpha.alpha = 0f;
		Kill.enabled = false;
	}

	public void TimerBarStart(float halftime){
		if (TimerBarActive == false) {
			TimerBar.enabled = true;
			TimerBarActive = true;
			TimerActiveTime = halftime * 2;
			StartCoroutine (TimerActive);
		}
	}
	public void TimerBarStop(){
		TimerBarActive = false;
		StopCoroutine (TimerActive);
		TimerBar.enabled = false;
	}
	IEnumerator Timer(){
		while (TimerBarActive) {
			TimerBarScale = ((Time.time - LevelMaster.TimerSmallestTime) / TimerActiveTime);
			TimerBar.rectTransform.offsetMax = new Vector2 (-TimerBarScale*1920,TimerBar.rectTransform.offsetMax.y);
			TimerBar.color = Color.HSVToRGB((1-TimerBarScale)/4, 1, 1);
			if (TimerBarScale > 1) {
				TimerBarStop ();
			}
			yield return new WaitForSeconds (0.03f);
		}
	}

	IEnumerator KillBar (){
		while (KillBarAlpha.alpha <1) {
			KillBarAlpha.alpha += 0.1f;
			yield return new WaitForSeconds (Time.deltaTime);
		}
	}
	IEnumerator ShotAccuracyCounter() {
		AccuracyText [0].enabled = true;
		AccuracyText [1].enabled = true;
		yield return new WaitForSeconds (1);
		ShotAccuracyOperator = 0;
		while (ShotAccuracyOperator < ShotAccuracy) {
			ShotAccuracyOperator += 1;
			AccuracyText[0].text = ShotAccuracyOperator.ToString() + "%";
			yield return new WaitForSeconds (Time.deltaTime);
		}
		yield return new WaitForSeconds (2f);
		AccuracyText[0].text = " ";
		AccuracyText [0].enabled = false;
		AccuracyText [1].enabled = false;
	}
}
