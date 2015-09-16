using UnityEngine;
using System.Collections;

public class AIWolf : MonoBehaviour 
{

	private Animator animator;

	private GameObject[] players;
	private GameObject closestPlayer;
	private Movement movement;

	public float clawAttackCD = 5f;
	public float lungeAttackCD = 15f;
	private float clawAttackRange = 1.8f;
	private float lungeAttackRange = 2.5f;
	private float clawAttackTimer;
	private float lungeAttackTimer;

	private float clawAttackDuration = 1.958f;
	private float lungeAttackDuration = 3.0f;
	public bool isClawAttacking = false;
	public bool isLungeAttacking = false;
	private bool isAttacking = false;
	private bool isIdle = false;
	
	public enum State
	{
		Chasing,
		BackingOff,
		Attacking,
		Idle,
	};
	public State state;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
		movement = GetComponent<Movement> ();
		players = GameObject.FindGameObjectsWithTag ("Player");

		clawAttackTimer = clawAttackCD;
		lungeAttackTimer = lungeAttackCD;
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
		case State.Idle:
			Idle();
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

		bool canAttack = (clawAttackTimer >= clawAttackCD) || (lungeAttackTimer >= lungeAttackCD);
		bool inRangeForAttack = ((closestPlayerDist <= clawAttackRange) && (clawAttackTimer >= clawAttackCD)) || ((closestPlayerDist <= lungeAttackRange) && (lungeAttackTimer >= lungeAttackCD));
		//bool inChaseRange = closestPlayerDist >= 1;

		bool isAttacking = isClawAttacking || isLungeAttacking;
		bool isChasing = canAttack && !inRangeForAttack && !isAttacking;
;
		//Check if the distance to the closest player is inside any of our thresholds
		//update state accordingly
		animator.SetBool ("IsChasing", isChasing);
		animator.SetBool ("IsBackingOff", !isChasing);
		animator.SetBool("IsIdle", isIdle);

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

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.name == "WolfCollider") 
		{
			isIdle = true;
		}
	}

	void Chase()
	{
		//figure out whether the target player is in front of or behind the Wolf and move in the
		//correct direction.
		isIdle = false;
		float diff = (closestPlayer.transform.position.x - transform.position.x);

		if (diff > 0) 
		{
			movement.SetDeltaMovement (movement.speed, 0.0f);
			movement.RotateToFace (Movement.Direction.Right);
		} 
		else 
		{
			movement.SetDeltaMovement (movement.speed*-1, 0.0f);
			movement.RotateToFace (Movement.Direction.Left);
		}
	}

	void Attack()
	{
		isIdle = false;
		if (!isAttacking)
		{
			float closestPlayerDist = (transform.position - closestPlayer.transform.position).sqrMagnitude;

			if ((clawAttackTimer >= clawAttackCD) && (closestPlayerDist <= clawAttackRange)) 
			{
				isClawAttacking = true;
				animator.SetTrigger ("ClawAttack");
				Invoke ("updateAttackBooleans", clawAttackDuration);
				clawAttackTimer = 0;
			}
			else
			{
				isLungeAttacking = true;
				animator.SetTrigger ("LungeAttack");
				Invoke ("updateAttackBooleans", lungeAttackDuration);
				lungeAttackTimer = 0;
				//LungeAttackMovement ();
			}
		}
	}

	void updateAttackBooleans()
	{
		isClawAttacking = false;
		isLungeAttacking = false;
	}
	void BackOff()
	{
		//isIdle = false;
		float diff = (closestPlayer.transform.position.x - transform.position.x);
		
		if (diff < 0) 
		{
			movement.SetDeltaMovement (movement.speed, 0.0f);
			movement.RotateToFace (Movement.Direction.Left);
		} 
		else 
		{
			movement.SetDeltaMovement ((movement.speed*-1), 0.0f);
			movement.RotateToFace (Movement.Direction.Right);
		}
	}

	void OnDestroy()
	{

	}
	


	void Idle()
	{
		animator.SetBool ("IsChasing", false);
		animator.SetBool ("IsBackingOff", false);
	}
}
