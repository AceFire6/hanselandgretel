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


	public float respawnDelay = 1.5f;

	[HideInInspector]
	public int maxHealth;


	private bool isAwaitingRespawn = false;
	private float respawnTimer = 0.0f;

	private GameObject otherPlayer = null;


	void Start() {
		maxHealth = totalHealth;

		if (gameObject.tag == "Player") {
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject p in players) {
				if (p.name != this.gameObject.name) {
					otherPlayer = p;
					break;
				}
			}
		}
	}

	public float GetHealthPercent() {
		return ((float)totalHealth / (float)maxHealth);
	}

	void Update() {
		if(isAwaitingRespawn){
			respawnTimer += Time.deltaTime;
			if (respawnTimer >= respawnDelay) {
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

				SendRespawnEvent();
			}
		}
	}

	public void TakeDamage (int damage)
	{
		totalHealth -= damage;

		if (totalHealth <= 0) {
			Die();
		}
	}

	public void Kill() {
		Die ();
	}

	private void Die()
	{

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
			}else{
				isAwaitingRespawn = true;

				gameObject.GetComponent<CharacterController> ().detectCollisions = false;
				Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer r in renderers) {
					r.enabled = false;
				}
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
