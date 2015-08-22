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

//used to test health script
//	void Update() {
//		if(Input.GetKeyUp(KeyCode.A)){
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

	private void Die()
	{

		if (deathSpawn != null){
			Vector3 pos = transform.position;
			pos.y += 0.4f;
			Instantiate(deathSpawn, pos, deathSpawn.transform.rotation);
		}

		Destroy(this.gameObject);
	}
}
