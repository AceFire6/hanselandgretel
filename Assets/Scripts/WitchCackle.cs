using UnityEngine;
using System.Collections;

public class WitchCackle : MonoBehaviour {

	public float cackleDelay = 5.0f; //seconds between cackles;
	public float cackleVariance = 0.0f; //allowable variance in cackleDelay

	public AudioClip[] cackleClips;
	private AudioSource[] cackleAuds;
	
	private AudioManager audioManager;

	private float cackleTimer = 0.0f;

	private AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}

	private void initAudio () {
		audioManager = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioManager>();

		if (cackleClips != null) {
			cackleAuds = new AudioSource[cackleClips.Length];

			for (int i = 0; i < cackleClips.Length; i++) {
				cackleAuds[i] = AddAudio (cackleClips[i], false, false, 1);
			}
		}
	}

	void Start () {
		initAudio ();
	}

	void Update () {
		cackleTimer += Time.deltaTime;

		float variance = Random.Range (-cackleVariance, cackleVariance);

		if (cackleTimer >= (cackleDelay + variance) && cackleAuds != null ) {
			//stop any cackle currently playing (theres only one witch)
			for (int i = 0; i < cackleAuds.Length; i++) {
				if (cackleAuds[i].isPlaying) {
					cackleAuds[i].Stop();
				}
			}

			//play a random cackle from our list of cackles
			if(!audioManager.isSoundMute){
				int cackleIndex = Random.Range (0, cackleAuds.Length-1);

				cackleAuds[cackleIndex].volume = audioManager.soundVolume;
				cackleAuds[cackleIndex].Play();
			}

			cackleTimer = 0;
		}
	}
}
