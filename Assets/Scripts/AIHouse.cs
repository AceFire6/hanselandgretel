using UnityEngine;
using System.Collections;

public class AIHouse : Movement
{

	private GameObject[] players;
	private GameObject closestPlayer;

	public bool isActive = false;

	public float leftBound;
	public float rightBound;

	public Transform[] pointLights;
	public float jumpLeeway = 1f;

	private Health health;

	private Animator animator;

	private float stompCD = 4f;
	private float jumpCD = 12f;

	private float stompDuration = 2.375f;
	private float jumpDuration = 2.417f;
	private float warmingUpTime = 7.1f;

	private float stompRange = 3f;
	private float jumpRange = 5.5f;
	private float rageDuration = 2.1f;

	private float stompTimer;
	private float jumpTimer;
	private float rageTimer;

	private float jumpFrame = 0f;

	private bool isIdle = false;
	private bool isAttacking = false;
	private bool isChasing = false;
	private bool isBackingOff = false;
	private bool isRaging = false;
	private bool isDying = false;

	public bool isStompAttacking = false;
	public bool isJumpAttacking = false;

	private float prevY;
	public float heightDiff;
	private float jumpEndFrame = 1.18f;

	private bool enragedModeOn = false;
	private bool canActivateRageMode = true;

	private float rotateTimer;
	private float rotateCD = 0.25f;
	public float growRate = 0.4f;
	public float currScale = 0.80f;

	private float maxHealth;

	public enum State
	{
		WarmingUp,
		Chasing,
		BackingOff,
		Attacking,
		Jumping,
		Idle,
		Raging,
		Dying
	};

	public State state;

	// Use this for initialization
	void Start () 
	{
		base.Start ();
		prevY = transform.position.y;
		players = GameObject.FindGameObjectsWithTag ("Player");
		animator = GetComponent<Animator> ();
		health = GetComponent<Health> ();

		stompTimer = stompCD;
		jumpTimer = jumpCD;
		rageTimer = 0f;
		rotateTimer = rotateCD;
		maxHealth = health.totalHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isActive) {
			base.Update ();
		
			if (canActivateRageMode && health.totalHealth <= maxHealth / 2) {
				canActivateRageMode = false;
				enragedModeOn = true;
				isRaging = true;
				animator.SetTrigger ("rageTrigger");
				jumpDuration = 3.75f;
				jumpTimer = jumpCD;
				stompCD = 3f;
				jumpCD = 10f;
				jumpRange = 22f;
				stompRange = 6f;
				base.speed = 0.9f;
				//animator.animation["Base Layer.BH_StompWalkBackwards"].speed = 2f;
				//animator.animation["Base Layer.BH_StompWalkForward"].speed = 2f;
				jumpEndFrame = jumpDuration - jumpLeeway;
				for (int i = 0; i < pointLights.Length; i++) {
					pointLights [i].gameObject.GetComponent<ParticleSystem> ().Play ();
					pointLights [i].gameObject.GetComponent<ParticleSystem> ().enableEmission = true;
				}
			}

			stompTimer += Time.deltaTime;
			jumpTimer += Time.deltaTime;
			jumpFrame += Time.deltaTime;
			rotateTimer += Time.deltaTime;

			warmingUpTime -= Time.deltaTime;
			UpdateState ();
			ExecuteState ();
		
			heightDiff = transform.position.y - prevY;
			prevY = transform.position.y;

			if (transform.localScale == new Vector3 (1.0f, 1.0f, 1.0f))
				transform.localScale = new Vector3 (currScale, currScale, currScale);
		}
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
		bool jumping = jumpFrame >= 0.30 && jumpFrame <= jumpEndFrame;

		animator.SetBool ("stompAttacking",isStompAttacking);
		animator.SetBool ("jumpAttacking", isJumpAttacking);
		animator.SetBool ("chasing", isChasing);
		animator.SetBool ("backingOff", !isChasing && !isAttacking);
		animator.SetBool ("idle", isIdle);

		if (isJumpAttacking) {
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("PlayerCharacter"), true);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("PlayerWeapon"), true);
		}
		else if (!collider.bounds.Contains(players[0].transform.position) && !collider.bounds.Contains(players[1].transform.position))
		{
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("PlayerCharacter"), false);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("PlayerWeapon"), false);
		}

		if (isDying)
			state = State.Dying;
		else if (isRaging) 
		{
			state = State.Raging;
		}
		else if (warmingUp) 
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
			case State.Dying:
				Die ();
				break;
			case State.Raging:
				Rage();
				break;
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
		//Find the closest player to the House
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
			if (rotateTimer >= rotateCD)
			{
				base.RotateToFace (Movement.Direction.Right);
				rotateTimer = 0f;
			}
		} 
		else 
		{
			base.SetDeltaMovement (base.speed*-1, 0.0f);
			if (rotateTimer >= rotateCD)
			{
				base.RotateToFace (Movement.Direction.Left);
				rotateTimer = 0f;
			}
		}
	}

	void BackOff()
	{
		isIdle = false;
		float diff = (closestPlayer.transform.position.x - transform.position.x);
		
		if (diff < 0) 
		{
			base.SetDeltaMovement (base.speed, 0.0f);
			if (rotateTimer >= rotateCD)
			{
				base.RotateToFace (Movement.Direction.Left);
				rotateTimer = 0f;
			}
		} 
		else 
		{
			base.SetDeltaMovement ((base.speed*-1), 0.0f);
			if (rotateTimer >= rotateCD)
			{
				base.RotateToFace (Movement.Direction.Right);
				rotateTimer = 0f;
			}
		}
	}

	void Rage()
	{
		rageTimer += Time.deltaTime;
		currScale += growRate * Time.deltaTime;
		transform.localScale = new Vector3(currScale,currScale,currScale);
		if (rageTimer >= rageDuration)
			isRaging = false;
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
				if (enragedModeOn)
					objRigidbody.velocity += new Vector3(0,15f,0);
				jumpFrame = 0f;
			}
		}
	}

	void Jump()
	{	
		isIdle = false;
		bool inBounds = transform.position.x <= rightBound && transform.position.x >= leftBound;
		if (jumpFrame <= jumpEndFrame) //&& inBounds) 
		{
			float diff = (closestPlayer.transform.position.x - transform.position.x);
			
			if (diff > 0) {
				base.SetDeltaMovement (base.speed * 5f, 0.0f);
				if (rotateTimer >= rotateCD)
				{
					base.RotateToFace (Movement.Direction.Right);
					rotateTimer = 0f;
				}
			} else {
				base.SetDeltaMovement (base.speed * -5f, 0.0f);
				if (rotateTimer >= rotateCD)
				{
					base.RotateToFace (Movement.Direction.Left);
					rotateTimer = 0f;
				}
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
	//	if (collision.gameObject.tag == "Player" || collision.gameObject.name == "AxeCollider" )
	//		Physics.IgnoreCollision (collision.collider, collider);
	}
	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.name == "HouseCollider") 
		{
			isIdle = true;
		}
	}

	public void Die()
	{
		if (!isDying)
		{
			isDying = true;
			GameObject obj = GameObject.Find("Blob Shadow Projector");
			Destroy(obj);
			animator.SetTrigger ("die");
			Destroy (gameObject, 6.25f);
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
