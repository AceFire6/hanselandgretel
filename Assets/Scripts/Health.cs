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

	private int maxHealth;


	void Start() {
		maxHealth = totalHealth;
	}

	public float GetHealthPercent() {
		return ((float)totalHealth / (float)maxHealth);
	}
//used to test health script
//	void Update() {
//		if(Input.GetKeyDown(KeyCode.X)){
//			TakeDamage(10);
//		}
//	}

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

		if (this.tag != "Player") {
			Destroy(this.gameObject);
		} else {
			totalHealth = maxHealth;
			gameObject.transform.position = GameObject.Find("GameManager").GetComponent<GameManager>().GetRespawnLocation();
			SendRespawnEvent ();
		}
	}

	void SendRespawnEvent () {
		GameObject[] all = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach (GameObject g in all) {
			g.SendMessage("OnPlayerRespawn", SendMessageOptions.DontRequireReceiver);
		}
	}
}
