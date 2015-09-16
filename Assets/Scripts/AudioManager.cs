

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

	public void AdjustMusicVolume (float vol){
		musicVolume = Mathf.Clamp01 (vol);

		backgroundMusic.volume = musicVolume;
	}

	public void ToggleMusicMute (bool isMute) {
		isMusicMute = isMute;
		backgroundMusic.mute = isMute;
	}

	public void AdjustSoundVolume (float vol){
		soundVolume = Mathf.Clamp01 (vol);
	}
	
	public void ToggleSoundMute (bool isMute) {
		isSoundMute = isMute;
	}
}
