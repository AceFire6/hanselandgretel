using UnityEngine;
using System.Collections;

public class AIHouse : Movement
{

	private GameObject[] players;
	private GameObject closestPlayer;

	private Animator animator;

	private float stompCD = 5f;
	private float jumpCD = 12f;

	private float stompDuration = 2.375f;
	private float jumpDuration = 2.417f;

	private float stompRange = 2f;
	private float jumpRange = 5f;

	private float stompTimer;
	private float jumpTimer;

	private bool isIdle = false;
	private bool isAttacking = false;
	private bool isChasing = false;
	private bool isBackingOff = false;

	private bool isStompAttacking = false;
	private bool isJumpAttacking = false;

	public enum State
	{
		Chasing,
		BackingOff,
		Attacking,
		Idle
	};

	public State state;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		animator = GetComponent<Animator> ();

		stompTimer = stompCD;
		jumpTimer = jumpCD;
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();

		stompTimer += Time.deltaTime;
		jumpTimer += Time.deltaTime;
		
		UpdateState ();
		ExecuteState ();
	}

	//Checks if a state transition is needed and updates currentState accordingly.
	//State changes are triggered by player proximity to the wolf.
	private void UpdateState ()
	{
		UpdateClosestPlayer ();

		float closestPlayerDist = (transform.position - closestPlayer.transform.position).sqrMagnitude;

		bool canAttack = (stompTimer >= stompCD) || (jumpTimer >= jumpCD);
		bool inRangeForAttack = ((closestPlayerDist <= stompRange) && (stompTimer >= stompCD)) || ((closestPlayerDist <= jumpRange) && (jumpTimer >= jumpCD));
		//bool inChaseRange = closestPlayerDist >= 1;
		
		bool isAttacking = isStompAttacking || isJumpAttacking;
		bool isChasing = canAttack && !inRangeForAttack && !isAttacking;

		//Check if the distance to the closest player is inside any of our thresholds
		//update state accordingly
		animator.SetBool ("chasing", isChasing);
		animator.SetBool ("backingOff", !isChasing);
		animator.SetBool("idle", isIdle);

		if (canAttack && inRangeForAttack && !isAttacking) 
		{
			state = State.Attacking;
		} 
		else if (isChasing) 
		{
			state = State.Chasing;
		}
		else if (!isAttacking && !isIdle)
		{
			state = State.BackingOff;
		}
		else 
		{
			state = State.Idle;
		}
	}

	//Checks the current state and executes the appropriate method.
	private void ExecuteState ()
	{
		switch (state) 
		{
			case State.Chasing:
				Chase ();
				break;
			case State.BackingOff:
				BackOff ();
				break;
			case State.Attacking:
				Attack ();
				break;
			case State.Idle:
				Idle();
				break;
		}
	}

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
		isIdle = false;

		float diff = (closestPlayer.transform.position.x - transform.position.x);
		
		if (diff > 0) 
		{
			base.SetDeltaMovement (base.speed, 0.0f);
			base.RotateToFace (Movement.Direction.Right);
		} 
		else 
		{
			base.SetDeltaMovement (base.speed*-1, 0.0f);
			base.RotateToFace (Movement.Direction.Left);
		}
	}

	void BackOff()
	{
		isIdle = false;

		float diff = (closestPlayer.transform.position.x - transform.position.x);
		
		if (diff < 0) 
		{
			base.SetDeltaMovement (base.speed, 0.0f);
			base.RotateToFace (Movement.Direction.Left);
		} 
		else 
		{
			base.SetDeltaMovement ((base.speed*-1), 0.0f);
			base.RotateToFace (Movement.Direction.Right);
		}
	}

	void Attack()
	{
		isIdle = false;

		if (!isAttacking)
		{
			float closestPlayerDist = (transform.position - closestPlayer.transform.position).sqrMagnitude;
			
			if ((stompTimer >= stompCD) && (closestPlayerDist <= stompRange)) 
			{
				isStompAttacking = true;
				animator.SetBool ("stompAttacking",true);
				Invoke ("updateAttackBooleans", stompDuration);
				stompTimer = 0;
			}
			else
			{
				isJumpAttacking = true;
				animator.SetBool ("jumpAttacking", true);
				Invoke ("updateAttackBooleans", jumpDuration);
				jumpTimer = 0;
			}
		}
	}

	void Idle()
	{

	}

	void updateAttackBooleans()
	{
		isStompAttacking = false;
		isJumpAttacking = false;
	}
}
