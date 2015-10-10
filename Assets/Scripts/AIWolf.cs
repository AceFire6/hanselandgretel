using UnityEngine;
using System.Collections;

public class AIWolf : Movement
{

	private Animator animator;

	private GameObject[] players;
	private GameObject closestPlayer;

	private Health health; //Checking hp to perform the take hit animation at certain intervals

	public float clawAttackCD = 5f;
	public float lungeAttackCD = 15f;
	private float clawAttackRange = 1.81f;
	private float lungeAttackRange = 2.3f;
	private float clawAttackTimer;
	private float lungeAttackTimer;

	private float clawAttackDuration = 1.958f;
	private float lungeAttackDuration = 3.0f;
	public bool isClawAttacking = false;
	public bool isLungeAttacking = false;
	private bool isAttacking = false;
	private bool isIdle = false;

	private bool isDying = false;

	private int lastHpHit;
	public enum State
	{
		Chasing,
		BackingOff,
		Attacking,
		Idle,
		Dying
	};
	public State state;

	private AudioManager audioManager;

	public AudioClip clawAttackClip;
	private AudioSource clawAttackAud;

	public AudioClip lungeAttackClip;
	private AudioSource lungeAttackAud;

	private AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
	
	private void initAudio () {
		audioManager = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioManager>();

		if (clawAttackClip != null) {
			clawAttackAud = AddAudio (clawAttackClip, false, false, 1);
		}

		if (lungeAttackClip != null) {
			lungeAttackAud = AddAudio (lungeAttackClip, false, false, 1);
		}
	}

	// Use this for initialization
	void Start () 
	{
		base.Start ();
		animator = GetComponent<Animator> ();
		health = GetComponent<Health>();
		players = GameObject.FindGameObjectsWithTag ("Player");

		lastHpHit = health.totalHealth;
		clawAttackTimer = clawAttackCD;
		lungeAttackTimer = lungeAttackCD;

		initAudio ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();
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
		case State.Dying:
			Die();
			break;
		}
	}

	//Checks if a state transition is needed and updates currentState accordingly.
	//State changes are triggered by player proximity to the wolf.
	private void UpdateState ()
	{
		int hpLeft = health.totalHealth;
		if ((hpLeft % 250 == 0) && (lastHpHit != hpLeft))
		{	
			lastHpHit -= 250;
			animator.SetTrigger("TakeHit");
		}

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

		if (isDying) 
		{
			state = State.Dying;
		}
		else if (canAttack && inRangeForAttack && !isAttacking) 
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
		 	base.SetDeltaMovement (base.speed, 0.0f);
			base.RotateToFace (Movement.Direction.Right);
		} 
		else 
		{
			base.SetDeltaMovement (base.speed*-1, 0.0f);
			base.RotateToFace (Movement.Direction.Left);
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

				//play clawAttack sound
				if(clawAttackAud != null && !clawAttackAud.isPlaying && !audioManager.isSoundMute){
					clawAttackAud.volume = audioManager.soundVolume;
					clawAttackAud.Play();
				}

				animator.SetTrigger ("ClawAttack");
				Invoke ("updateAttackBooleans", clawAttackDuration);
				clawAttackTimer = 0;
			}
			else
			{
				isLungeAttacking = true;

				//play lungeAttack sound
				if(lungeAttackAud != null && !lungeAttackAud.isPlaying && !audioManager.isSoundMute){
					lungeAttackAud.volume = audioManager.soundVolume;
					lungeAttackAud.Play();
				}

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
			base.SetDeltaMovement (base.speed, 0.0f);
			base.RotateToFace (Movement.Direction.Left);
		} 
		else 
		{
			base.SetDeltaMovement ((base.speed*-1), 0.0f);
			base.RotateToFace (Movement.Direction.Right);
		}
	}

	public void Die()
	{
		if (!isDying)
		{
			isDying = true;
			animator.SetTrigger ("Die");
			Destroy (gameObject, 4f);
		}
	}

	void Idle()
	{
		animator.SetBool ("IsChasing", false);
		animator.SetBool ("IsBackingOff", false);
	}
}
