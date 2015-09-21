/*
 * GameManager script to control the spawning of AI at different spawn points and update the amount 
 * of coins the player possesses.
 * 
 * Author: Aashiq Parker
 * Date: 22-August-2015
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour {

	public Transform minionPrefab;
	public int limit;

	private int coins;
	private Text CoinCount;
	private Vector3 StartLocation;
	private int lastCheckpoint;
	private Transform[] spawnPoints;
	private Transform[] minions;
	private int[] spawnLimits;
	private PlayerSettings settings;

	void Start () 
	{
		string levelName = EditorApplication.currentScene.Replace("Assets/Levels/", "").Replace (".unity", "");
		settings = GameObject.Find("SettingsController").GetComponent<PlayerSettings>();
		while (!settings.Loaded) {}
		if (settings.MostRecentLevel == levelName) {
			StartLocation = settings.GetLastCheckpointPosition();
			foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
				player.transform.position = StartLocation;
			}
		} else {
			StartLocation = GameObject.FindGameObjectsWithTag ("Player")[0].transform.position;
			settings.MostRecentLevel = levelName;
			settings.SetLastCheckpoinPosition(StartLocation);
			settings.SaveSettings();
		}
		coins = settings.Coins;
		UpdateCoins(0);

		Transform cam = GameObject.Find ("Camera").transform;
		Vector3 cameraPos = cam.position;
		cam.position = new Vector3(StartLocation.x, cameraPos.y, cameraPos.z);

		spawnPoints = GetComponentsInChildren<Transform> ();
		minions = new Transform[spawnPoints.Length];
		limit = 0;
		CoinCount = GameObject.Find("Coins").GetComponent<Text>();
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
		return StartLocation;
	}

	public void UpdateCoins(int amount)
	{
		coins += amount;
		CoinCount.text = "Coins: " + coins;
	}
}
