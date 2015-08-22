/*
 * GameManager script to control the spawning of AI at different spawn points and update the amount 
 * of coins the player possesses.
 * 
 * Author: Aashiq Parker
 * Date: 22-August-2015
 */

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Transform minionPrefab;
	public int coins;

	private Transform[] spawnPoints;
	private Transform[] minions;

	void Start () 
	{
		spawnPoints = GetComponentsInChildren<Transform> ();
		minions = new Transform[spawnPoints.Length];
		coins = 0;
		UpdateMinions ();
	}

	void Update () 
	{
		UpdateMinions ();
	}

	/*Loops through minion array and spawns a minion at the spawn points if the current one is dead*/
	void UpdateMinions()
	{
		for (int i = 1; i < minions.Length; i++) //Starts at 1 because GetComponentsInChildren includes the parent object
		{
			if (minions[i] == null)
			{	
				Vector3 position = spawnPoints[i].position;
				position.x = position.x + Random.Range (-3.5f,3.5f); //Add some randomness to where it gets spawned
				minions[i] = (Transform)Instantiate(minionPrefab,position, spawnPoints[i].rotation);
									//Can also use Invoke to add a delay before spawning the minion but it makes the 
											//code a bit uglier
			}
		}
	}

	void UpdateCoints(int amount)
	{
		coins += amount;
		Debug.Log ("Coins: " + coins);
	}
}
