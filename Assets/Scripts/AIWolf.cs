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

	public float movementSpeed = 1f;

	private enum State
	{
		Chasing,
		BackingOff,
		Attacking,
		Idle,
	};
	private State state;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator> ();
		movement = GetComponent<Movement> ();
		players = GameObject.FindGameObjectsWithTag ("Player");

		clawAttackTimer = 3f;
		lungeAttackTimer = 5f;
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

		bool canAttack = (clawAttackTimer >= clawAttackCD);
		bool inRangeForAttack =(closestPlayerDist <= clawAttackRange);
		//bool inChaseRange = closestPlayerDist >= 1;

		bool isAttacking = animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.LungeAndBite") || animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.ClawSwipe");  
		bool isChasing = canAttack && !inRangeForAttack && !isAttacking;
		Debug.Log (isAttacking);
		//Check if the distance to the closest player is inside any of our thresholds
		//update state accordingly

		animator.SetBool ("IsChasing", isChasing);
		animator.SetBool ("IsBackingOff", !isChasing);

		if ((canAttack && inRangeForAttack) || isAttacking) 
		{
			state = State.Attacking;
		} 
		else if (isChasing) 
		{
			state = State.Chasing;
		}
		else if (!isAttacking)
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

	void Chase()
	{
		//figure out whether the target player is in front of or behind the Wolf and move in the
		//correct direction.
		float diff = (closestPlayer.transform.position.x - transform.position.x);

		if (diff > 0) 
		{
			movement.SetDeltaMovement (movementSpeed, 0.0f);
			movement.RotateToFace (Movement.Direction.Right);
		} 
		else 
		{
			movement.SetDeltaMovement (movementSpeed*-1, 0.0f);
			movement.RotateToFace (Movement.Direction.Left);
		}
	}

	void Attack()
	{
		if (clawAttackTimer >= clawAttackCD)
			animator.SetTrigger ("ClawAttack");
		clawAttackTimer = 0;
	}

	void BackOff()
	{
		float diff = (closestPlayer.transform.position.x - transform.position.x);
		
		if (diff < 0) 
		{
			movement.SetDeltaMovement (movementSpeed/2, 0.0f);
			movement.RotateToFace (Movement.Direction.Left);
		} 
		else 
		{
			movement.SetDeltaMovement ((movementSpeed*-1)/2, 0.0f);
			movement.RotateToFace (Movement.Direction.Right);
		}
	}

	void OnDestroy()
	{

	}

	void ClawAttack()
	{
		animator.SetTrigger ("ClawAttack");
	}

	void LungeAttack()
	{

	}

	void Idle()
	{
		animator.SetBool ("IsChasing", false);
		animator.SetBool ("IsBackingOff", false);
	}
}
