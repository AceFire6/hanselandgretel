/*
 * Health script allows GameObject to take damage and die. The deathSpawn
 * member is a prefab that will be spawned at the location where the GameObject dies.
 * Set DeathSpawn to Coin for minions, portal for bosses, and null for players.
 * 
 * Author: Muhummad Patel
 * Date: 17-August-2015
 */

using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int totalHealth = 100;
	public GameObject deathSpawn; //object to be spawned when this GameObject dies

	public AudioClip deathClip;
	private AudioSource deathAud;

	public AudioClip[] hurtClip;
	private AudioSource[] hurtAud;

	public float respawnDelay = 1.5f;

	[HideInInspector]
	public int maxHealth;


	private bool isAwaitingRespawn = false;
	private float respawnTimer = 0.0f;

	private bool skipDeath;

	private GameObject otherPlayer = null;

	private AudioManager audioManager;

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

		if (hurtClip.Length != 0) {
			hurtAud = new AudioSource[hurtClip.Length];
			for(int i = 0; i < hurtClip.Length; i++){
				hurtAud[i] = AddAudio(hurtClip[i], false, false, 1);
			}
		}

		if (deathClip != null) {
			deathAud = AddAudio (deathClip, false, false, 1);
		}
	}

	void Start() {
		int difficulty = GameObject.Find("SettingsController").GetComponent<PlayerSettings>().DifficultyIndex;
		maxHealth = totalHealth;

		if (gameObject.tag == "Player") {
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

			foreach (GameObject p in players) {
				if (p.name != this.gameObject.name) {
					otherPlayer = p;
					break;
				}
			}
		} else {
			maxHealth += (int)(difficulty * (maxHealth / 4.0));
			totalHealth = maxHealth;
		}
		respawnDelay += difficulty;

		initAudio ();
	}

	public float GetHealthPercent() {
		return ((float)totalHealth / (float)maxHealth);
	}

	void Update() {
		if (!isAwaitingRespawn && skipDeath) {
			skipDeath = false;
		}
		if(isAwaitingRespawn){
			respawnTimer += Time.deltaTime;
			gameObject.transform.position = otherPlayer.transform.position;
			if (respawnTimer >= respawnDelay || skipDeath) {
				skipDeath = false;
				respawnTimer = 0.0f;
				isAwaitingRespawn = false;

				totalHealth = maxHealth;

				Vector3 respawnPos = otherPlayer.transform.position;
				respawnPos.x -= 0.7f;
				gameObject.transform.position = respawnPos;

				gameObject.GetComponent<CharacterController> ().detectCollisions = true;
				Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer r in renderers) {
					r.enabled = true;
				}

				AudioSource[] audioSources = gameObject.GetComponentsInChildren<AudioSource>();
				foreach (AudioSource a in audioSources) {
					a.enabled = true;
				}

				gameObject.GetComponents<MonoBehaviour>()[0].enabled = true;

				SendRespawnEvent();
			}
		}
	}

	public void TakeDamage (int damage)
	{
		totalHealth -= damage;

		if (totalHealth <= 0) {
			Die ();
		} else {
			//only got hurt, so play the hurt sound
			if(hurtAud != null && !audioManager.isSoundMute){
				int i = Random.Range(0, hurtAud.Length);
				hurtAud[i].volume = audioManager.soundVolume;
				hurtAud[i].Play();
			}
		}
	}

	public void Kill() {
		Die ();
	}

	private void Die()
	{
		//Dying, so play the die sound(if any)
		if (deathAud != null && !audioManager.isSoundMute) {
			deathAud.volume = audioManager.soundVolume;
			deathAud.Play();
		}

		if (deathSpawn != null){
			Vector3 pos = transform.position;
			pos.y += 0.4f;
			Instantiate(deathSpawn, pos, deathSpawn.transform.rotation);
		}

		if (this.name == "BBWolf_Unity") //Change later to check a tag for boss
			gameObject.GetComponent<AIWolf> ().Die();
		else if (this.tag != "Player") {
			Destroy(this.gameObject);
		} else {
			if(otherPlayer == null){
				totalHealth = maxHealth;
				gameObject.transform.position = GameObject.Find("GameManager").GetComponent<GameManager>().GetRespawnLocation();
				SendRespawnEvent ();
			} else if (otherPlayer.GetComponent<Health>().isAwaitingRespawn) {
				Vector3 respawnLoc = GameObject.Find("GameManager").GetComponent<GameManager>().GetRespawnLocation();
				otherPlayer.transform.position = respawnLoc;
				gameObject.transform.position = respawnLoc;

				SendRespawnEvent();

				otherPlayer.GetComponent<Health>().skipDeath = true;
			}else{
				isAwaitingRespawn = true;

				gameObject.GetComponent<CharacterController> ().detectCollisions = false;
				Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer r in renderers) {
					r.enabled = false;
				}

				AudioSource[] audioSources = gameObject.GetComponentsInChildren<AudioSource>();
				foreach (AudioSource a in audioSources) {
					a.enabled = false;
				}

				// Temporary fix!
				gameObject.GetComponents<MonoBehaviour>()[0].enabled = false;
			}
		}
	}

	void SendRespawnEvent () {
		GameObject[] all = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach (GameObject g in all) {
			g.SendMessage("OnPlayerRespawn", SendMessageOptions.DontRequireReceiver);
		}
	}
}
