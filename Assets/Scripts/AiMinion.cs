using UnityEngine;
using System.Collections;

public class AiMinion : MonoBehaviour {

	Movement movement;

	enum State {Wandering, Chasing, Attacking};
	State current;
	private GameObject[] players;
	float chaseRadius = 6.0f;
	float attackRadius = 3.0f;
	float wanderTime = 2.0f;
	float timeWandering = 0.0f;

	// Use this for initialization
	void Start () {
		movement = (Movement)GetComponent<Movement>();
		current = State.Wandering;
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		UpdateState();
		ExecuteState();
	}

	void UpdateState(){
		Vector3 minDist = transform.position - players[0].transform.position;
//		Debug.Log (minDist);
		foreach (GameObject player in players){
			if((transform.position - player.transform.position).sqrMagnitude < minDist.sqrMagnitude){
				minDist = transform.position - player.transform.position;
			}
		}

		float closest = minDist.sqrMagnitude;

		if (closest < attackRadius){
			current = State.Attacking;
		}else if (closest < chaseRadius){
			current = State.Chasing;
		}else{
			current = State.Wandering;
		}
	}

	void ExecuteState(){
		switch (current){
			case State.Wandering:
				Wander();
				break;
			case State.Chasing:
				Chase();
				break;
			case State.Attacking:
				Attack ();
				break;
		}
	}

	bool dir = false;
	void Wander(){
		timeWandering += Time.deltaTime;

		if(timeWandering > wanderTime){
			timeWandering = 0.0f;
			movement.SetDeltaRotation(0.0f, 180.0f);
			dir = !dir;
		}

		if(dir){
		movement.SetDeltaMovement(1.0f, 0.0f);
		}else{
			movement.SetDeltaMovement(-1.0f, 0.0f);
		}
	}

	void Chase(){}
	void Attack(){}
}
