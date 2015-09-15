using UnityEngine;
using System.Collections;

public class AIWolf : MonoBehaviour 
{

	private Animator animator;

	private GameObject[] players;
	private GameObject closestPlayer;
	private Movement movement;

	public float clawAttackCD = 3f;
	public float lungeAttackCD = 5f;
	private float clawAttackRange = 5f;
	private float lungeAttackRange = 10f;
	private float clawAttackTimer;
	private float lungeAttackTimer;

	private enum State
	{
		Chasing,
		BackingOff,
		Attacking,
		Dead
	};
	private State state;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
		movement = GetComponent<Movement> ();
		players = GameObject.FindGameObjectsWithTag ("Player");

		clawAttackTimer = 0f;
		lungeAttackTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		clawAttackTimer += Time.deltaTime;
		lungeAttackTimer += Time.deltaTime;

		UpdateState ();
		ExecuteState ();
	}

	//Checks the current state and executes the appropriate method.
	private void ExecuteState ()
	{
		switch (state) {
		case State.Chasing:
			Chase ();
			break;
		case State.BackingOff:
			BackOff ();
			break;
		case State.Attacking:
			Attack ();
			break;
		case State.Dead:
			Die();
			break;
		}
	}

	//Checks if a state transition is needed and updates currentState accordingly.
	//State changes are triggered by player proximity to the wolf.
	private void UpdateState ()
	{
		//Get the distance to the closest player
		UpdateClosestPlayer ();
		float closestPlayerDist = (transform.position - closestPlayer.transform.position).sqrMagnitude;

		//Check if the distance to the closest player is inside any of our thresholds
		//update state accordingly
/*		if (closestPlayerDist < attackRadius) {
			currentState = State.Attacking;
		} else if (closestPlayerDist < chaseRadius) {
			currentState = State.Chasing;
		} else {
			currentState = State.Wandering;
		}*/
	}

	//Updates closestPlayer to reference the player that is closest to the wolf.
	private void UpdateClosestPlayer ()
	{
		//Find the closest player to the Wolf
		GameObject target = players [0];
		float distToTarget = (transform.position - players [0].transform.position).sqrMagnitude;
		foreach (GameObject player in players) 
		{
			float distToPlayer = (transform.position - player.transform.position).sqrMagnitude;
			if (distToPlayer < distToTarget) 
			{
				distToTarget = (transform.position - player.transform.position).sqrMagnitude;
				target = player;
			}
		}
		closestPlayer = target;
	}

	void Chase()
	{

	}

	void Attack()
	{
		animator.SetTrigger ("ClawAttack");
	}

	void BackOff()
	{

	}

	void Die()
	{

	}

	void ClawAttack()
	{

	}

	void LungeAttack()
	{

	}
}
