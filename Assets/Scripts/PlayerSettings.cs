using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerSettings : MonoBehaviour {

	[HideInInspector]
	public int DifficultyIndex;

	[HideInInspector]
	public int MusicVolume;

	[HideInInspector]
	public int SoundVolume;

	[HideInInspector]
	public int MuteMusic;

	[HideInInspector]
	public int MuteSound;

	[HideInInspector]
	public string MostRecentLevel;

	[HideInInspector]
	public string CheckpointPosition;

	[HideInInspector]
	public bool Started;

	[HideInInspector]
	public bool Loaded;

	[HideInInspector]
	public int Coins;

	private AudioManager audioManager;

	void Start () {
		if (!PlayerPrefs.HasKey("Played")) {
			FirstPlaySetup();
		}
		DifficultyIndex = PlayerPrefs.GetInt("Difficulty");


		audioManager = GameObject.FindGameObjectWithTag ("AudioController").GetComponent<AudioManager> ();
		MuteMusic = PlayerPrefs.GetInt("MuteMusic");
		audioManager.ToggleMusicMute (MuteMusic != 0);
		MuteSound = PlayerPrefs.GetInt("MuteSound");
		audioManager.ToggleSoundMute (MuteSound != 0);

		MusicVolume = PlayerPrefs.GetInt("MusicVolume");
		audioManager.AdjustMusicVolume (MusicVolume);
		SoundVolume = PlayerPrefs.GetInt("SoundVolume");
		audioManager.AdjustSoundVolume (SoundVolume);

		MostRecentLevel = PlayerPrefs.GetString("MostRecentLevel");
		CheckpointPosition = PlayerPrefs.GetString("CheckpointPosition");
		Coins = PlayerPrefs.GetInt("Coins");
		Started = true;
		Loaded = true;
	}

	void Update() {
		string levelName = Application.loadedLevelName;
		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel("Level1_2");
		} else if (Input.GetKeyDown(KeyCode.F2)) {
			Vector3 newPos = new Vector3(112F, 0F, 0F);
			foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
				player.transform.position = newPos;
			}
		} else if (Input.GetKeyDown(KeyCode.F3)) {
			Vector3 newPos = new Vector3(185F, 0.1F, 0F);
			foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
				player.transform.position = newPos;
			}
		}
	}

	void FirstPlaySetup() {
		PlayerPrefs.SetString("Played", "");
		
		PlayerPrefs.SetInt("Difficulty", 1);
		
		PlayerPrefs.SetInt("MuteMusic", 0);
		PlayerPrefs.SetInt("MuteSound", 0);
		
		PlayerPrefs.SetInt("MusicVolume", 100);
		PlayerPrefs.SetInt("SoundVolume", 100);
		
		PlayerPrefs.SetString("MostRecentLevel", "");
		PlayerPrefs.SetString("CheckpointPosition", "");

		PlayerPrefs.SetInt("Coins", 0);
		
		PlayerPrefs.Save();
	}

	public void NewGame() {
		PlayerPrefs.SetString("Played", "");
		PlayerPrefs.SetString("MostRecentLevel", "");
		PlayerPrefs.SetString("CheckpointPosition", "");
		PlayerPrefs.SetInt("Coins", 0);
		
		PlayerPrefs.Save();
	}

	public void SaveSettings() {
		PlayerPrefs.SetString("Played", "");

		PlayerPrefs.SetInt("Difficulty", DifficultyIndex);

		PlayerPrefs.SetInt("MuteMusic", MuteMusic);
		PlayerPrefs.SetInt("MuteSound", MuteSound);

		PlayerPrefs.SetInt("MusicVolume", MusicVolume);
		PlayerPrefs.SetInt("SoundVolume", SoundVolume);

		PlayerPrefs.SetString("MostRecentLevel", MostRecentLevel);
		PlayerPrefs.SetString("CheckpointPosition", CheckpointPosition);

		PlayerPrefs.SetInt("Coins", Coins);

		PlayerPrefs.Save();
	}

	public void SetSoundVolume() {
		SoundVolume = (int)GameObject.Find("Sound").GetComponentInChildren<Slider>().value;
		audioManager.AdjustSoundVolume (SoundVolume/100.0f);
	}

	public void SetMusicVolume() {
		MusicVolume = (int)GameObject.Find("Music").GetComponentInChildren<Slider>().value;
		audioManager.AdjustMusicVolume (MusicVolume/100.0f);
	}

	public void SetDifficultyIndex(string difficulty) {
		if (difficulty == "Easy") {
			DifficultyIndex = 0;
		} else if (difficulty == "Normal") {
			DifficultyIndex = 1;
		} else {
			DifficultyIndex = 2;
		}
	}

	public bool IsSoundMuted() {
		return (MuteSound == 1) ? true : false;
	}

	public bool IsMusicMuted() {
		return (MuteMusic == 1) ? true : false;
	}

	public void SetMuteSound(bool mute) {
		MuteSound = mute ? 1 : 0;
		audioManager.ToggleSoundMute (mute);
	}

	public void SetMuteMusic(bool mute) {
		MuteMusic = mute ? 1 : 0;
		audioManager.ToggleMusicMute (mute);
	}

	public void UpdateSoundMute() {
		SetMuteSound(GameObject.Find("MuteSound").GetComponent<Toggle>().isOn);
	}

	public void UpdateMusicMute() {
		SetMuteMusic(GameObject.Find("MuteMusic").GetComponent<Toggle>().isOn);
	}

	public void SetLastCheckpointPosition(Vector3 checkpoint, int coins) {
		CheckpointPosition = checkpoint.x + "," + checkpoint.y + "," + checkpoint.z;
		Coins = coins;
		PlayerPrefs.SetString("CheckpointPosition", CheckpointPosition);
		PlayerPrefs.SetInt("Coins", Coins);
		PlayerPrefs.Save();
	}

	public void SetLastCheckpointPosition(Vector3 checkpoint) {
		CheckpointPosition = checkpoint.x + "," + checkpoint.y + "," + checkpoint.z;
		PlayerPrefs.SetString("CheckpointPosition", CheckpointPosition);
		PlayerPrefs.Save();
	}

	public Vector3 GetLastCheckpointPosition() {
		string[] vals = CheckpointPosition.Split(',');
		float x = Convert.ToSingle(vals[0]);
		float y = Convert.ToSingle(vals[1]);
		float z = Convert.ToSingle(vals[2]);
		return  new Vector3(x, y, z);
	}
}
