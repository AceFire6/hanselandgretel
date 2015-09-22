/*
 * AudioManager script handles all sound related operations. It stores the nackgroundMusic
 * AudioSource, and adusts the music and sound volumes. It also handles muting the music
 * or sound effects. The adjustment methods are event callbacks for the settings menu GUI
 * elements (sound and music sliders and mute checkboxes).
 * 
 * Author: Muhummad Patel
 * Date: 15-September-2015
 */

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource backgroundMusic;

	private float musicVolume = 1.0f;
	private float soundVolume = 1.0f;

	private bool isMusicMute = false;
	private bool isSoundMute = false;

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
		//TODO: adjust volume of all audioSources
	}
	
	public void ToggleSoundMute (bool isMute) {
		isSoundMute = isMute;
		//TODO: mute all audioSources
	}
}
