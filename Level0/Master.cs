using UnityEngine;
using System.Collections;

public abstract class Master : MonoBehaviour { 
	[HideInInspector] public GameObject SpherePrefab;
	[HideInInspector] public GameObject HealthPrefab;
	[HideInInspector] public GameObject AmmoPrefab;
	[HideInInspector] public GameObject TriggerPrefab;
	[HideInInspector] public int EnemyKilled;
	[HideInInspector] public int TotalNumberOfEnemy;
	[HideInInspector] public int CurrentNumebrOfEnemy;
	[HideInInspector] public int numberOfEnemyAtOnce;
	protected int PlayerKills;
	protected int TriggerCount;
	protected int bombscount;
	protected int bombsatonce;
	protected int TriggerCountAim;
	protected int LevelStage = 0;
	protected string Stage;
	protected bool Spawning;
	protected bool bombspawning;
	protected bool HA;
	protected Coroutine StageCoroutine;
	protected MyCharacterController PlayerScript;
	protected bool FirstPass = false;
	protected int BombID;
	protected bool failed;
	public float[] TimerTimes;
	public Canvas BlackCanvas;
	public bool[] BombSpawnLocations = new bool[8];
	int BombSpawnLocationNew;
	CanvasGroup Canvas;
	public AudioMaster Audio;
	public GameObject Player;
	public LaserShot Weapon;
	public TouchInputs TI;
	public MainMenu Menu;
	public  Transform[] SpawnLocation;
	public float TimerSmallestTime = Mathf.Infinity;

	public void Initialize() {
		PlayerScript = Player.GetComponent<MyCharacterController> ();
		SpherePrefab = (GameObject) Resources.Load ("Sphere");
		enemyIAI SpherePrefabScript = SpherePrefab.GetComponent<enemyIAI> ();
		SpherePrefabScript.IsEnemy = true;
		SpherePrefabScript.Player = Player;
		SpherePrefabScript.M = gameObject.GetComponent<Master> ();
		HealthPrefab = (GameObject) Resources.Load("Health");
		Health HealthPrefabScript = HealthPrefab.gameObject.GetComponent<Health> ();
		HealthPrefabScript.M = gameObject.GetComponent<Master> ();
		HealthPrefabScript.PlayerScript = Player.GetComponent<MyCharacterController> ();
		AmmoPrefab = (GameObject) Resources.Load("Ammo");
		Ammo AmmoPrefabScript = AmmoPrefab.gameObject.GetComponent<Ammo> ();
		AmmoPrefabScript.M = gameObject.GetComponent<Master> ();
		AmmoPrefabScript.PlayerScript = Player.GetComponent<MyCharacterController> ();
		TriggerPrefab = (GameObject) Resources.Load("Trigger");
		Trigger TriggerPrefabScript = TriggerPrefab.gameObject.GetComponent<Trigger> ();
		TriggerPrefabScript.M = gameObject.GetComponent<Master> ();
		Canvas = BlackCanvas.GetComponent<CanvasGroup> ();

		if (GlobalVariables.LevelStage != 0) {
			PlayerScript.Health = GlobalVariables.PlayerCheckPointHealth;
			Player.transform.position = GlobalVariables.PlayerCheckPointPosition;
			Player.transform.eulerAngles = GlobalVariables.PlayerCheckPointEuler;
			Weapon.Ammo = GlobalVariables.AmmoChekckPoint;
		}
		StopAllCoroutines ();
		StartCoroutine (In ());
		LevelStage = GlobalVariables.LevelStage;
		Stage = "Stage" + LevelStage;
		StageCoroutine = StartCoroutine (Stage, 0);
	}
	public virtual void CheckPoint(){
		GlobalVariables.PlayerCheckPointHealth = PlayerScript.Health;
		GlobalVariables.PlayerCheckPointPosition = Player.transform.position;
		GlobalVariables.PlayerCheckPointEuler = Player.transform.eulerAngles;
		GlobalVariables.AmmoChekckPoint = Weapon.Ammo;
		GlobalVariables.SetVariables ();
	}
	public virtual void OnPlayerKill(){
		if (failed == false) {
			failed = true;
			PlayerKills += 1;
			TI.PlayerKill ();
			Audio.Stop ();
			StopAllCoroutines ();
			Spawning = false;
			bombspawning = false;
			Audio.PlayAdditional (0);
			StartCoroutine (RevivePlayer ());
		}
	}
	public virtual void OnEnemyKill() {
		GlobalVariables.Score += 1;
		EnemyKilled += 1;
		CurrentNumebrOfEnemy-= 1;
		TI.EnemyBar ();
	}
	public virtual void OnAmmoEmpty(){
	}
	public virtual void OnTriggerReach(){
		TriggerCount += 1;
		if (TriggerCount >= TriggerCountAim) {
			NewStage ();
		}  
	}

	protected void InitializeStage(){
		GameObject[] Triggers = GameObject.FindGameObjectsWithTag ("Trigger");
		foreach (GameObject T in Triggers) {
			Destroy (T);
		}
		for (int i = 0; i < BombSpawnLocations.Length; i++) {
			BombSpawnLocations [i] = false;
		}
		if (bombsatonce > 0) {
			TimerTimes = new float[bombsatonce];
			for (int i = 0; i < TimerTimes.Length; i++) {
				TimerTimes [i] = Mathf.Infinity;
			}
		}
		FirstPass = true;
		Audio.Stop ();
		EnemyKilled = 0;
		TriggerCount = 0;
		bombscount = 0;
		BombID = 0;
		TimerSmallestTime = Mathf.Infinity;
		TI.HealthBar ();
		TI.EnemyBar ();
		TI.AmmoUpdate ();
		TI.TimerBarStop ();
		GlobalVariables.LevelStage = LevelStage;
	}
	protected void NewStage() {
		LevelStage += 1;
		StopCoroutine (StageCoroutine);
		Stage = "Stage" + LevelStage;
		FirstPass = false;
		PlayerKills = 0;
		StageCoroutine = StartCoroutine (Stage,0);
	}
	public IEnumerator In(){
		Canvas.alpha = 1;
		while (Canvas.alpha >= 0) {
			Canvas.alpha -= 0.2f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}
	public IEnumerator Out(){
		Canvas.alpha = 0;
		while (Canvas.alpha < 1) {
			Canvas.alpha += 0.2f*Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}
	/*protected IEnumerator StopListening() {
		while (AudioListener.volume > 0.2f && PlayerScript.dead){
			AudioListener.volume -= 0.01f;
			yield return new WaitForSeconds (Time.deltaTime);
		}
		print ("stopListening");
		yield return new WaitForSeconds (4);
		AudioListener.volume = 1;
	}
	*/
	protected IEnumerator RevivePlayer(){
		yield return new WaitForSeconds (5);
		CurrentNumebrOfEnemy = 0;
		PlayerScript.Health = GlobalVariables.PlayerCheckPointHealth;
		Player.transform.position = GlobalVariables.PlayerCheckPointPosition;
		Player.transform.eulerAngles = GlobalVariables.PlayerCheckPointEuler;
		Weapon.Ammo = GlobalVariables.AmmoChekckPoint;
		TI.PlayerRevive ();
		FirstPass = false;
		failed = false;
		PlayerScript.dead = false;
		StageCoroutine = StartCoroutine (Stage,0);
	}
	protected IEnumerator InstantiateEnemy (Vector2 range) {
		Spawning = true; 
		while (Spawning) {
			if (CurrentNumebrOfEnemy < numberOfEnemyAtOnce) {
				if (TotalNumberOfEnemy != 0 && (EnemyKilled + CurrentNumebrOfEnemy) < TotalNumberOfEnemy) {
					Instantiate (SpherePrefab, SpawnLocation [Random.Range (Mathf.RoundToInt(range.x), Mathf.RoundToInt(range.y))].position, transform.rotation);
					CurrentNumebrOfEnemy += 1;
				}
			//	if (TotalNumberOfEnemy == 0 ) {
			//		Instantiate (SpherePrefab, SpawnLocation [Random.Range (Mathf.RoundToInt(range.x), Mathf.RoundToInt(range.y))].position, transform.rotation);
			//		CurrentNumebrOfEnemy += 1;
			//	}
			} else {
				Spawning = false;
			}
			TI.EnemyBar ();
			yield return new WaitForSeconds (2);
		}
	}
	protected IEnumerator HealthAmmoSpawner() {
		yield return new WaitForSeconds (5);
		while (true) {
			if (!HA) {
				GameObject HP = (GameObject) Instantiate (HealthPrefab, SpawnLocation[Random.Range(0,7)].position, Quaternion.Euler (Vector3.up));
				HP.GetComponent<Health> ().isFlying = false;
				HA = true;
			} else {
				GameObject AP = (GameObject) Instantiate (AmmoPrefab, SpawnLocation[Random.Range(0,7)].position, Quaternion.Euler (Vector3.up));
				AP.GetComponent<Ammo> ().isFlying = false;
				HA = false;
			}
			yield return new WaitForSeconds (5);
		}
	}
	protected IEnumerator BombSpawner(Vector3 range) {
		bombspawning = true;
		while (bombspawning) {
			yield return new WaitForSeconds (1);
			if (bombsatonce > bombscount) {
				BombSpawnLocationNew = Random.Range (Mathf.RoundToInt (range.x), Mathf.RoundToInt (range.y));
				if (BombSpawnLocations [BombSpawnLocationNew] == false) {
					for (int i = 0; i < TimerTimes.Length; i++) {
						if (TimerTimes [i] == Mathf.Infinity) {
							BombID = i;
						}
					}
					GameObject B = (GameObject)Instantiate (TriggerPrefab, (SpawnLocation [BombSpawnLocationNew].position), Quaternion.Euler (Vector3.up));
					Trigger TriggerBombScript = B.gameObject.GetComponent<Trigger> ();
					TriggerBombScript.bomb = true;
					TriggerBombScript.halftime = range.z;
					TriggerBombScript.ID = BombID;
					TriggerBombScript.OccupiedLocation = BombSpawnLocationNew;
					TimerTimes [BombID] = Time.time;
					if (Mathf.Min (TimerTimes) < Mathf.Infinity) {
						TimerSmallestTime = Mathf.Min (TimerTimes);
						TI.TimerBarStart (range.z);
					} else {
						TI.TimerBarStop ();
					}
					bombscount += 1;
					BombSpawnLocations [BombSpawnLocationNew] = true;
				}
			} else {
				bombspawning = false;
			}
			yield return new WaitForSeconds (1);
		}
	}
	protected void Resume(){
		Time.timeScale = 1;
		TI.StopMove ();
		TI.StopRotate ();
		Audio.UnPause();
		AudioListener.volume = 1f;
	}
}
