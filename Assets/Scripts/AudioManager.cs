using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	AudioSource backgroundMusic;

	private float musicVolume = 1.0f;
	private float soundVolume = 1.0f;

	private bool isMusicMute = false;
	private bool isSoundMute = false;

	void Start () {
		backgroundMusic = GetComponent<AudioSource> ();
	}

	void Update () {
	
	}

	public void AdjustMusicVolume (float vol){
		vol = Mathf.Clamp01 (vol);

		backgroundMusic.volume = vol;
	}

	public void ToggleMusicMute (bool isMute) {
		isMusicMute = isMute;
		backgroundMusic.mute = isMute;
	}
}
