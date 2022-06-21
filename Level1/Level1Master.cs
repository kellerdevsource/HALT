using UnityEngine;
using System.Collections;

public class Level1Master : Master {
	// Use this for initialization
	void Awake(){
		GlobalVariables.CurrentLevel = 1;
	}
	void Start() {
		Initialize ();
		StartCoroutine (In ());
		if (GlobalVariables.HighestReachedLevel < 1) {
			GlobalVariables.HighestReachedLevel = 1;
		} 
	}
	void Update(){
	}
	public override void OnPlayerKill(){
		base.OnPlayerKill ();
		GameObject[] AllEnemy = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject EnemyInstance in AllEnemy) {
			Destroy (EnemyInstance, 0);
		}
	}
	public override void OnTriggerReach() {
		base.OnTriggerReach ();
		if (bombscount > 0) {
			if (Mathf.Min (TimerTimes) < Mathf.Infinity) {
				TimerSmallestTime = Mathf.Min (TimerTimes);
			} else {
				TI.TimerBarStop ();
			}
			bombscount -= 1;
		}
		StageCoroutine = StartCoroutine (Stage, 0);
	}
	public override void OnEnemyKill() {
		base.OnEnemyKill();
		if (!Spawning) {
			StartCoroutine (InstantiateEnemy(new Vector2 (0, 15)));
		}
		if (EnemyKilled == TotalNumberOfEnemy) {
			NewStage ();
		} else {
			StageCoroutine = StartCoroutine (Stage, 1);
		}
	}
	public override void OnAmmoEmpty(){
	}
	/*public override void OnTriggerReach(){
		base.OnTriggerReach ();
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject E in Enemies) {
			Destroy (E);
		}
	}
	*/
	public IEnumerator Stage0(int localtype){
		if (!FirstPass) {
			TotalNumberOfEnemy = 1;
			TriggerCountAim = 1;
			numberOfEnemyAtOnce = 0;
			bombsatonce = 0;
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			Instantiate (TriggerPrefab, SpawnLocation [0].position, Quaternion.Euler (Vector3.up));
		}
			yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage1(int localtype){
		if (!FirstPass) {
			TriggerCountAim = 2;
			TotalNumberOfEnemy = 1;
			numberOfEnemyAtOnce = 0;
			bombsatonce = 0;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			GameObject Ammo = (GameObject)Instantiate (AmmoPrefab, SpawnLocation [1].position, Quaternion.Euler (Vector3.up));
			Ammo.GetComponent<Ammo> ().isFlying = false;
			Ammo.GetComponent<Ammo> ().isDestroyable = false;
			GameObject Health = (GameObject)Instantiate (HealthPrefab, SpawnLocation [2].position, Quaternion.Euler (Vector3.up));
			Health.GetComponent<Health> ().isFlying = false;
			Health.GetComponent<Health> ().isDestroyable = false;
			Instantiate (TriggerPrefab, SpawnLocation [1].position, Quaternion.Euler (Vector3.up));
			Instantiate (TriggerPrefab, SpawnLocation [2].position, Quaternion.Euler (Vector3.up));
		}
			yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage2(int localtype) {
		if (!FirstPass) {
			TriggerCountAim = 1;
			TotalNumberOfEnemy = 1;
			numberOfEnemyAtOnce = 0;
			bombsatonce = 1;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForSeconds (1);
			Audio.Play (LevelStage);
			yield return new WaitForSeconds (11);
			if (!bombspawning) {
				StartCoroutine (BombSpawner (new Vector3 (6, 6, 20)));
			}
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage3(int localtype) {
		if (!FirstPass) {
			TotalNumberOfEnemy = 1;
			numberOfEnemyAtOnce = 1;
			bombsatonce = 0;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			yield return new WaitForSeconds (7);
			Audio.PlayAdditional (1);
			if (!Spawning) {
				StartCoroutine (InstantiateEnemy (new Vector2 (7, 7)));
			}
		}
			yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage4(int localtype) {
		if (!FirstPass) {
			TriggerCountAim = 5;
			TotalNumberOfEnemy = 1;
			numberOfEnemyAtOnce = 0;
			bombsatonce = 1;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForSeconds (1);
			Audio.Play (LevelStage);
			yield return new WaitForSeconds (7);
		}
		if (!bombspawning && localtype == 0) {
			StartCoroutine (BombSpawner (new Vector3 (0, 8, 20)));
		}
		if ((TriggerCount == (TriggerCountAim - 1)) && localtype == 0) {
			yield return new WaitForSeconds (1.5f);
			Audio.PlayAdditional (8);
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage5(int localtype) {
		if (!FirstPass) {
			TotalNumberOfEnemy = 10;
			numberOfEnemyAtOnce = 3;
			bombsatonce = 0;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			StartCoroutine (HealthAmmoSpawner ());
			yield return new WaitForSeconds (6);
			Audio.PlayMusic (0);
			if (!Spawning) {
				StartCoroutine (InstantiateEnemy (new Vector2 (0, 15)));
			}
		}
			if (EnemyKilled == 3) {
				yield return new WaitForSeconds (2);
				Audio.PlayAdditional (6);
			}
			if (EnemyKilled == 5) {
				Audio.PlayAdditional (3);
			}
			if (EnemyKilled == 10) {
				Audio.PlayMusicRandom ();
			}
		if (EnemyKilled == (TotalNumberOfEnemy - 1)) {
			yield return new WaitForSeconds (1);
			Audio.PlayAdditional (8);
		}
			yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage6(int localtype) {
		if (!FirstPass) {
			TotalNumberOfEnemy = 20;
			numberOfEnemyAtOnce = 5;
			TriggerCountAim = 1000000;
			bombsatonce = 1;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			StartCoroutine (HealthAmmoSpawner ());
			yield return new WaitForSeconds (4);
			Audio.PlayMusicRandom ();
			if (!Spawning) {
				StartCoroutine (InstantiateEnemy (new Vector2 (0, 15)));
			}

		}
		if (!bombspawning && localtype == 0) {
			StartCoroutine (BombSpawner (new Vector3 (0, 8, 30)));
		}
		if ((EnemyKilled == (TotalNumberOfEnemy - 1)) && localtype == 1) {
			yield return new WaitForSeconds (1);
			Audio.PlayAdditional (8);
		}
		if (TriggerCount == 2 && localtype == 0) {
			yield return new WaitForSeconds (3);
			Audio.PlayAdditional (9);
		}
		if ((TriggerCount == (TriggerCountAim - 1)) && localtype == 0) {
			yield return new WaitForSeconds (1);
			Audio.PlayAdditional (8);
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage7(int localtype) {
		if (!FirstPass) {
			TotalNumberOfEnemy = 20;
			numberOfEnemyAtOnce = 5;
			TriggerCountAim = 1000000;
			bombsatonce = 2;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			StartCoroutine (HealthAmmoSpawner ());
			yield return new WaitForSeconds (4);
			Audio.PlayMusicRandom ();
			if (!Spawning) {
				StartCoroutine (InstantiateEnemy (new Vector2 (0, 15)));
			}
		}
		if (!bombspawning && localtype == 0) {
			StartCoroutine (BombSpawner (new Vector3 (0, 8, 45)));
		}
		if ((EnemyKilled == (TotalNumberOfEnemy - 1)) && localtype == 1) {
			yield return new WaitForSeconds (1);
			Audio.PlayAdditional (8);
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage8(int localtype) {
		if (!FirstPass) {
			TotalNumberOfEnemy = 200;
			numberOfEnemyAtOnce = 7;
			TriggerCountAim = 1000000;
			bombsatonce = 3;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			Audio.Play (LevelStage);
			StartCoroutine (HealthAmmoSpawner ());
			yield return new WaitForSeconds (6);
			Audio.PlayMusicRandom ();
			if (!Spawning) {
				StartCoroutine (InstantiateEnemy (new Vector2 (0, 15)));
			}
		}
		if (!bombspawning && localtype == 0) {
			StartCoroutine (BombSpawner (new Vector3 (0, 8, 45)));
		}
		if (((EnemyKilled % 25 == 0) && (EnemyKilled>1)) && localtype == 1) {
		}
		if ((EnemyKilled == (TotalNumberOfEnemy - 1)) && localtype == 1) {
			yield return new WaitForSeconds (1);
			Audio.PlayAdditional (8);
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage9(int localtype) {
		if (!FirstPass) {
			yield return new WaitForFixedUpdate();
			Menu.FinalMenuActivate ();
			TotalNumberOfEnemy = 1000000;
			numberOfEnemyAtOnce = 10;
			TriggerCountAim = 1000000;
			bombsatonce = 3;
			CheckPoint ();
			InitializeStage ();
			yield return new WaitForFixedUpdate();
			StartCoroutine (HealthAmmoSpawner ());
			yield return new WaitForSeconds (6);
			Audio.PlayMusicRandom ();
			if (!Spawning) {
				StartCoroutine (InstantiateEnemy (new Vector2 (0, 15)));
			}
		}
		if (!bombspawning && localtype == 0) {
			StartCoroutine (BombSpawner (new Vector3 (0, 8, 45)));
		}
		if ((EnemyKilled == (TotalNumberOfEnemy - 1)) && localtype == 1) {
			yield return new WaitForSeconds (1);
			Audio.PlayAdditional (8);
		}
		yield return new WaitForSeconds (1);
	}
	// Update is called once per frame
}
