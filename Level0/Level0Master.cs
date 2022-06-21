using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level0Master : Master {
	Vector3 Buffer;
	Vector3 Scale;
	public Circle C;
	void Awake(){
		GlobalVariables.CurrentLevel = 0;
	}
	void Start() {
		Initialize ();
		TI.yrotate = 0.35f;
		StartCoroutine (In ());
		if (GlobalVariables.HighestReachedLevel < 0) {
			GlobalVariables.HighestReachedLevel = 0;
		}
	}
	public override void OnPlayerKill(){
		base.OnPlayerKill ();
	//	C.redo (Scale);
	}
	public override void CheckPoint(){
		base.CheckPoint ();
		Scale = C.transform.localScale;
	}
	public IEnumerator Stage0(int localtype){
		if (!FirstPass) {
			TotalNumberOfEnemy = 1;
			TriggerCountAim = 0;
			InitializeStage ();
			yield return new WaitForSeconds (10);
			Audio.Play (LevelStage);
			yield return new WaitForSeconds (47);
			NewStage ();
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage1(int localtype){
		if (!FirstPass) {
			TotalNumberOfEnemy = 1;
			TriggerCountAim = 1;
			InitializeStage ();
			CheckPoint ();
			Audio.Play (LevelStage);
			yield return new WaitForSeconds (15f);
			GameObject TriggerCopy = Instantiate (TriggerPrefab, SpawnLocation [0].position, Quaternion.Euler (Vector3.up));
			BoxCollider TriggerCol = TriggerCopy.GetComponent<BoxCollider> ();
			TriggerCol.size = new Vector3 (0.35f, 0.35f, 0.35f);
		}
		yield return new WaitForSeconds (1);
	}
	public IEnumerator Stage2(int localtype){
		if (!FirstPass) {
			PlayerScript.locked = true;
			TotalNumberOfEnemy = 1;
			TriggerCountAim = 1;
			InitializeStage ();
			CheckPoint ();
			Audio.Play (LevelStage);
			yield return new WaitForSeconds (5);
			C.speed = 2;
			C.moove();
			yield return new WaitForSeconds (5);
			while (PlayerScript.gameObject.transform.position.y < 9) {
				PlayerScript.gameObject.transform.position = Vector3.SmoothDamp(PlayerScript.gameObject.transform.position, new Vector3(SpawnLocation[0].position.x, (PlayerScript.gameObject.transform.position.y + 2) , SpawnLocation[0].position.z),ref Buffer, 5f);
				//PlayerScript.gameObject.transform.position = new Vector3(PlayerScript.gameObject.transform.position.x,	(PlayerScript.gameObject.transform.position.y + 0.5f * Time.deltaTime),	PlayerScript.gameObject.transform.position.z);
				yield return new WaitForEndOfFrame ();
			}
			Menu.Loading.enabled = true;
			yield return new WaitForEndOfFrame ();
			GlobalVariables.LevelStage = 0;
			SceneManager.LoadScene ("level1");
		}
		yield return new WaitForSeconds (1);
	}
	/*public IEnumerator Stage3(){
		TotalNumberOfEnemy = 1;
		TriggerCountAim = 1;
		InitializeStage ();
		//Audio.Play (LevelStage);
		yield return StartCoroutine (Out ());
		GlobalVariables.LevelStage = 0;
		SceneManager.LoadScene ("level1");
		yield return new WaitForSeconds (1);
	}
	*/
}
