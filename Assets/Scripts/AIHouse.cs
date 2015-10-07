using UnityEngine;
using System.Collections;

public class AIHouse : Movement
{

	private GameObject[] players;
	private GameObject closestPlayer;

	private Animator animator;

	private float stompCD = 4f;
	private float jumpCD = 13.5f;

	private float stompDuration = 2.375f;
	private float jumpDuration = 2.417f;
	private float warmingUpTime = 7.1f;

	private float stompRange = 2f;
	private float jumpRange = 11f;

	private float stompTimer;
	private float jumpTimer;

	private float jumpFrame = 0f;

	private bool isIdle = false;
	private bool isAttacking = false;
	private bool isChasing = false;
	private bool isBackingOff = false;

	public bool isStompAttacking = false;
	public bool isJumpAttacking = false;

	public enum State
	{
		WarmingUp,
		Chasing,
		BackingOff,
		Attacking,
		Jumping,
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
		jumpFrame += Time.deltaTime;

		warmingUpTime -= Time.deltaTime;
		
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

		bool isAttacking = isStompAttacking || isJumpAttacking;
		bool isChasing = canAttack && !inRangeForAttack && !isAttacking;
		bool warmingUp = warmingUpTime >= 0;
		bool jumping = jumpFrame >= 0.30 && jumpFrame <= 1.18;

		//Debug.Log (state);
		animator.SetBool ("stompAttacking",isStompAttacking);
		animator.SetBool ("jumpAttacking", isJumpAttacking);
		animator.SetBool ("chasing", isChasing);
		animator.SetBool ("backingOff", !isChasing && !isAttacking);
		animator.SetBool ("idle", isIdle);

		if (warmingUp) 
		{
			state = State.WarmingUp;
		}
		else if (jumping)
		{
			state = State.Jumping;
		}
		else if ((canAttack && inRangeForAttack) && !	isAttacking) 
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
			case State.WarmingUp:
				break;
			case State.Chasing:
				Chase ();
				break;
			case State.BackingOff:
				BackOff ();
				break;
			case State.Jumping:
				Jump();
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
				Invoke ("updateAttackBooleans", stompDuration);
				stompTimer = stompDuration * -1;
			}
			else
			{
				isJumpAttacking = true;
				//objRigidbody.AddForce(new Vector3(0,7,0));
				Invoke ("updateAttackBooleans", jumpDuration);
				jumpTimer = jumpDuration * -1;
				jumpFrame = 0f;
			}
		}
	}

	void Jump()
	{	
		isIdle = false;
		float diff = (closestPlayer.transform.position.x - transform.position.x);
		
		if (diff > 0) {
			base.SetDeltaMovement (base.speed * 5f, 0.0f);
			base.RotateToFace (Movement.Direction.Right);
		} else {
			base.SetDeltaMovement (base.speed * -5f, 0.0f);
			base.RotateToFace (Movement.Direction.Left);
		}
	}

	void Idle()
	{
		animator.SetBool("backingOff", false);
	}

	void updateAttackBooleans()
	{
		isStompAttacking = false;
		isJumpAttacking = false;
	}
}
