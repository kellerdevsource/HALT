using UnityEngine;
using System.Collections;

public class AudioMaster : MonoBehaviour {
	public AudioSource[] AudioS;
	public AudioSource[] AudioSmusic;
	public AudioClip[] MasterSounds;
	public AudioClip[] MasterSoundsAdditional;
	public AudioClip[] MasterMusic;
	IEnumerator RandomMusic;
	int i;
	int r;
	bool playrandom;
	void Start(){
		RandomMusic = RandomM();
	}
	public void Play(int ClipNumber) {
		for (i = 0; i<AudioS.Length; i++) {
			AudioS[i].Stop();
			AudioS[i].PlayOneShot(MasterSounds[ClipNumber]);
		}
	}
	public void PlayAdditional(int ClipNumber){
		for (i = 0; i<AudioS.Length; i++) {
			AudioS [i].PlayOneShot (MasterSoundsAdditional [ClipNumber]);
		}
	}
	public void PlayMusic(int ClipNumber){
		for (i = 0; i < AudioSmusic.Length; i++) {
			AudioSmusic [i].PlayOneShot (MasterMusic [ClipNumber]);
		}
	}
	public void PlayMusicRandom(){
		StartCoroutine(RandomMusic);
	}
	public void Pause() {
		for (i = 0; i < AudioS.Length; i++) {
			AudioS [i].Pause();
		}
		for (i = 0; i<AudioSmusic.Length; i++) {
			AudioSmusic[i].Pause();
		}
	}
	public void UnPause() {
		for (i = 0; i < AudioS.Length; i++) {
			AudioS [i].UnPause();
		}
		for (i = 0; i<AudioSmusic.Length; i++) {
			AudioSmusic[i].UnPause();
		}
	}
	IEnumerator RandomM() {
		playrandom = true;
		while (playrandom) {
			r = Random.Range (0, MasterMusic.Length);
			for (i = 0; i < AudioSmusic.Length; i++) {
				if (!AudioSmusic [i].isPlaying) {
					AudioSmusic [i].PlayOneShot (MasterMusic [r]);
				}
			}
			yield return new WaitForSeconds (120);
		}
	}
	public void Stop() {
		if (playrandom) {
			StopCoroutine (RandomMusic);
			playrandom = false;
		}
		for (i = 0; i<AudioS.Length; i++) {
			AudioS[i].Stop();
		}
		for (i = 0; i<AudioSmusic.Length; i++) {
			AudioSmusic[i].Stop();
		}
	}
}
