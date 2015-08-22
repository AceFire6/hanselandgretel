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

	private Transform[] spawnPoints;
	private Transform[] minions;
	GameObject obj;

	void Start () 
	{
		spawnPoints = GetComponentsInChildren<Transform> ();
		minions = new Transform[spawnPoints.Length];
		minions[0] = (Transform)Instantiate (minionPrefab, transform.position, transform.rotation);
	}

	void Update () 
	{
	
	}

}
