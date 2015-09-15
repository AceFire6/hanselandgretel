using UnityEngine;
using System.Collections;

public class FallingSpike : MonoBehaviour {

	public float activationRange = 1.0f;
	public float vibrationIntensity = 0.025f;
	public float vibrateDelay = 0.5f;
	public float vibrationVariance = 0.2f;

	private Vector3 spikeOrigin;
	private Rigidbody rigidbody;
	private GameObject[] players;

	private enum State {Passive, Vibrating, Falling, Grounded};
	private State state = State.Passive;

	private float vibrationTimer = 0.0f;

	void Start () {
		spikeOrigin = transform.position;
		rigidbody = GetComponent<Rigidbody> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	void Update () {
		UpdateState ();
		ExecuteState ();
	}

	void UpdateState () {
		if (state == State.Passive && PlayerInRange ()) {
			state = State.Vibrating;
		}else if (state == State.Vibrating){
			vibrationTimer += Time.deltaTime;

			if (vibrationTimer >= vibrateDelay) {
				state = State.Falling;
			}
		}
	}

	void ExecuteState () {
		switch (state) {
		case State.Vibrating:
			Vibrate ();
			break;
		case State.Falling:
			Fall ();
			break;
		}
	}

	void Vibrate () {
		float deltaX = Random.Range(-vibrationIntensity, vibrationIntensity);
		Vector3 newPos = spikeOrigin;
		newPos.x += deltaX;
		
		transform.position =  newPos;
	}

	void Fall () {
		rigidbody.isKinematic = false;
	}

	void OnCollisionEnter(Collision collision)
	{		
		GameObject obj = collision.gameObject;
		if (obj.tag != "Player" && state != State.Grounded)  
		{

			Vector3 pos = rigidbody.position;
			pos.y -= 0.2f;
			rigidbody.isKinematic = true;
			rigidbody.MovePosition (pos);
			//collider.isTrigger = true; //Prevent it from colliding with other objects
			//active = false;
			transform.parent = collision.transform;

			state = State.Grounded;

		} else if (state != State.Grounded) {
			collision.gameObject.GetComponent<Health>().Kill();
		}

	}
	
	bool PlayerInRange () {
		float distToClosest = float.MaxValue;
		foreach (GameObject p in players) {
			float distToP = spikeOrigin.x - p.transform.position.x;
			if (distToP < distToClosest) {
				distToClosest = distToP;
			}
		}

		return (distToClosest <= activationRange);
	}
}
