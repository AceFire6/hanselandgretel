/*
 * Controls the bahviour of the falling stalactites in the Level1_2 scene. Uses a state machine
 * to keep track of the state of the spike. The spike gets activated when the player gets within
 * the activationRange, and then begins vibrating. After waiting for vibrationDelay seconds, the
 * spike will then fall down on the player. The spike will only kill the player if it hits the
 * player while it is falling. At other times, it will allow collisions, but will not be harmful.
 * 
 * Author: Muhummad Patel
 * Date: 15-September-2015
 */

using UnityEngine;
using System.Collections;

public class FallingSpike : MonoBehaviour {

	public float activationRange = 1.0f; //starts vibrating when player is closer than this
	public float vibrationIntensity = 0.025f; //how much the spike vibrates
	public float vibrateDelay = 0.5f; //how long to vibrate before falling
	public float disappearDelay = 2.0f; //how long the spikes stay on screen after landing before they disappear

	private Vector3 spikeOriginPos; //original location (vibrates around this point)
	private Quaternion spikeOriginRot; //original rotation of the spike
	private Rigidbody rbody;
	private GameObject[] players;

	private enum State {Passive, Vibrating, Falling, Grounded};
	private State state = State.Passive;

	private float vibrationTimer = 0.0f;
	private float disappearTimer = 0.0f;

	//Initialise required variables
	void Start () {
		spikeOriginPos = transform.position;
		spikeOriginRot = transform.rotation;
		rbody = GetComponent<Rigidbody> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	void Update () {
		UpdateState ();
		ExecuteState ();
	}

	//Check if a state change is neccesary, and update the state accordingly
	void UpdateState () {
		if (state == State.Passive && PlayerInRange ()) {
			state = State.Vibrating;
		}else if (state == State.Vibrating){
			vibrationTimer += Time.deltaTime;

			if (vibrationTimer >= vibrateDelay) {
				state = State.Falling;
			}
		}else if (state == State.Grounded) {
			disappearTimer += Time.deltaTime;
		}
	}

	//Note: If we're passive or grounded, then there's nothing to really do.
	void ExecuteState () {
		switch (state) {
		case State.Vibrating:
			Vibrate ();
			break;
		case State.Falling:
			Fall ();
			break;
		case State.Grounded:
			Disappear();
			break;
		}
	}

	//Vibrate about the original position of the spike by some small amount.
	void Vibrate () {
		float deltaX = Random.Range(-vibrationIntensity, vibrationIntensity);
		Vector3 newPos = spikeOriginPos;
		newPos.x += deltaX;
		
		transform.position =  newPos;
	}

	//allow gravity to do it's work.
	void Fall () {
		rbody.isKinematic = false;
		rbody.AddTorque (new Vector3 (0, 1000, 0)); //slight spin as it falls.
	}

	//Disbale the mesh renderer and collision detection (makes spike basically invisible)
	void Disappear () {
		if (disappearTimer >= disappearDelay) {
			rbody.detectCollisions = false;
			this.gameObject.GetComponent <MeshRenderer> ().enabled = false;
		}
	}

	//Called when the player respawns (message sent from the players health script)
	//Resets the spike to it's original position and state.
	void OnPlayerRespawn () {
		disappearTimer = 0.0f;
		vibrationTimer = 0.0f;
		state = State.Passive;

		rbody.detectCollisions = true;
		rbody.isKinematic = true;
		rbody.MovePosition (spikeOriginPos);
		rbody.rotation = spikeOriginRot;


		this.gameObject.GetComponent <MeshCollider> ().enabled = true;
		this.gameObject.GetComponent <MeshRenderer> ().enabled = true;
	}
	
	void OnCollisionEnter(Collision collision)
	{		
		GameObject obj = collision.gameObject;
		if (obj.layer != LayerMask.NameToLayer("PlayerCharacter") && obj.layer != LayerMask.NameToLayer("PlayerWeapon") && obj.tag != "Minion" && state != State.Grounded)  
		{
			if (obj.name != "RoofCollider"){
				//if we hit the ground, sink slightly, and set state to grounded
				Vector3 pos = rbody.position;
				pos.y -= Random.Range(0.1f, 0.5f);
				rbody.isKinematic = true;
				rbody.MovePosition (pos);

				state = State.Grounded;
			}
		} else if (state == State.Falling) {
			//if we hit the player while falling, kill the player
			try{
				collision.gameObject.GetComponent<Health>().Kill();
			}catch(System.NullReferenceException e){
				//We hit something without a health component, so ignore
			}
		}

	}

	//check if a player is in range of the spike.
	bool PlayerInRange () {
		float distToClosest = float.MaxValue;
		foreach (GameObject p in players) {
			float distToP = spikeOriginPos.x - p.transform.position.x;
			if (distToP < distToClosest) {
				distToClosest = distToP;
			}
		}

		return (distToClosest <= activationRange);
	}
}
