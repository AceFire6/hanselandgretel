﻿/*
 * AI controller for the minion enemies. This script controls their movement
 * using a simple state-machine. The minions are either wandering, chasing, or
 * attacking.
 * 
 * Author: Muhummad Patel
 * Date: 17-August-2015
 */
using UnityEngine;
using System.Collections;

public class AiMinion : MonoBehaviour
{

	public float wanderTime = 2.0f; //time to wander in each direction before turning around
	public float attackTime = 1.0f; //interval between attacks in Attacking state
	public GameObject meleeSlash;

	public enum State
	{
		Wandering,
		Chasing,
		Attacking};
	private State currentState;
	
	private float chaseRadius = 6.0f;
	private float attackRadius = 1.0f;
	private float attackTimer = 0.0f;
	private float wanderingTimer = 0.0f;

	private GameObject[] players;
	private Movement movement;
	
	protected void Start ()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		movement = (Movement)GetComponent<Movement> ();
		currentState = State.Wandering;
	}

	protected void Update ()
	{
		UpdateState ();
		ExecuteState ();
	}

	//Checks if a state transition is needed and updates currentState accordingly.
	//State changes are triggered by player proximity to the minion.
	private void UpdateState ()
	{
		//Get the distance to the closest player
		float closestPlayerDist = (transform.position - players [0].transform.position).sqrMagnitude;
		foreach (GameObject player in players) {

			float distToPlayer = (transform.position - player.transform.position).sqrMagnitude;
			if (distToPlayer < closestPlayerDist) {
				closestPlayerDist = distToPlayer;
			}
		}

		//Check if the distance to the closest player is inside any of our thresholds
		//update state accordingly
		if (closestPlayerDist < attackRadius) {
			currentState = State.Attacking;
		} else if (closestPlayerDist < chaseRadius) {
			currentState = State.Chasing;
		} else {
			currentState = State.Wandering;
		}
	}

	//Checks the current state and executes the appropriate method.
	private void ExecuteState ()
	{
		switch (currentState) {
		case State.Wandering:
			Wander ();
			break;
		case State.Chasing:
			Chase ();
			break;
		case State.Attacking:
			Attack ();
			break;
		}
	}

	//The minion will move forward for wanderTime after which it will turn around and
	//move in the other direction for wanderTime.
	private void Wander ()
	{
		wanderingTimer += Time.deltaTime;

		//if the timer is up, then turn to face the other direction and reset the timer
		if (wanderingTimer > wanderTime) {
			wanderingTimer = 0.0f;
			if (movement.facing == Movement.Direction.Right) {
				movement.RotateToFace (Movement.Direction.Left);
			} else {
				movement.RotateToFace (Movement.Direction.Right);
			}
		}

		//move forward in the direction we are currently facing
		if (movement.facing == Movement.Direction.Right) {
			movement.SetDeltaMovement (1.0f, 0.0f);
		} else {
			movement.SetDeltaMovement (-1.0f, 0.0f);
		}
	}

	//The minion will move towards the closest player while they are still within the
	//chase radius.
	private void Chase ()
	{
		//Find the closest player to the minion
		GameObject target = players [0];
		float distToTarget = (transform.position - players [0].transform.position).sqrMagnitude;
		foreach (GameObject player in players) {
			float distToPlayer = (transform.position - player.transform.position).sqrMagnitude;
			if (distToPlayer < distToTarget) {
				distToTarget = (transform.position - player.transform.position).sqrMagnitude;
				target = player;
			}
		}

		//figure out whether the target player is in front of or behind the minion and move in the
		//correct direction.
		float diff = (target.transform.position.x - transform.position.x);
		if (diff > 0) {
			movement.SetDeltaMovement (1.0f, 0.0f);
			movement.RotateToFace (Movement.Direction.Right);
		} else {
			movement.SetDeltaMovement (-1.0f, 0.0f);
			movement.RotateToFace (Movement.Direction.Left);
		}
	}


	//The minon stops and attacks the player.
	//TODO: replace placeholder slashing with animated bite attack.
	private void Attack ()
	{
		//Find the closest player to the minion
		GameObject target = players [0];
		float distToTarget = (transform.position - players [0].transform.position).sqrMagnitude;
		foreach (GameObject player in players) {
			float distToPlayer = (transform.position - player.transform.position).sqrMagnitude;
			if (distToPlayer < distToTarget) {
				distToTarget = (transform.position - player.transform.position).sqrMagnitude;
				target = player;
			}
		}

		//figure out whether the target player is in front of or behind the minion and move in the
		//correct direction.
		float diff = (target.transform.position.x - transform.position.x);
		if (diff > 0) {
			movement.RotateToFace (Movement.Direction.Right);
		} else {
			movement.RotateToFace (Movement.Direction.Left);
		}

		//check timer and attack if it's time to attack and then reset timer.
		attackTimer += Time.deltaTime;
		if (attackTimer > attackTime) {
			//TODO: REPLACE THIS WITH ANIMATION CODE
			Vector3 pos = target.transform.position;
			pos.y += 0.5f;
			pos.z -= 0.3f;
			GameObject slash = (GameObject)Instantiate(meleeSlash, pos, meleeSlash.transform.rotation);
			Destroy(slash, 0.5f);

			attackTimer = 0;
		}
	}
}