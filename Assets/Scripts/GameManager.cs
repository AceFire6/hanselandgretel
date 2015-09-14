/*
 * GameManager script to control the spawning of AI at different spawn points and update the amount 
 * of coins the player possesses.
 * 
 * Author: Aashiq Parker
 * Date: 22-August-2015
 */

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Transform StartLocation;
	public Transform minionPrefab;
	public int coins;
	public int limit;
	public Text CoinCount;

	private int lastCheckpoint;
	private Transform[] spawnPoints;
	private Transform[] minions;
	private int[] spawnLimits;

	void Start () 
	{
		spawnPoints = GetComponentsInChildren<Transform> ();
		minions = new Transform[spawnPoints.Length];
		coins = 0;
		limit = 2;
		spawnLimits = new int[spawnPoints.Length];

		for (int i = 0; i < spawnLimits.Length; i++)
			spawnLimits [i] = limit;

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
			if (minions[i] == null && spawnLimits[i] > 0)
			{	
				Vector3 position = spawnPoints[i].position;
				position.x = position.x + Random.Range (-3.5f,3.5f); //Add some randomness to where it gets spawned
				minions[i] = (Transform)Instantiate(minionPrefab,position, spawnPoints[i].rotation);
				spawnLimits[i]--;
									//Can also use Invoke to add a delay before spawning the minion but it makes the 
											//code a bit uglier
			}
		}
	}

	public void SetLastCheckpoint(int id) {
		lastCheckpoint = id;
	}

	public Vector3 GetRespawnLocation() {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag ("Checkpoint")) {
			if (go.GetComponentInChildren<Checkpoint>().GetID() == lastCheckpoint) {
				return go.transform.position;
			}
		}
		return StartLocation.position;
	}

	public void UpdateCoins(int amount)
	{
		coins += amount;
		CoinCount.text = "Coins: " + coins;
	}
}
