using UnityEngine;
using System.Collections;

public class RaiseFence : MonoBehaviour {

	public float raiseSpeed = 0.7f; //speed at which the fence will rise
	public float depth = 0.5f; //How far to sink/rise
	public float rightTriggerDist = 0.7f; //How far must the players have walked past us to trigger the gate to rise
	public float cameraZoomedOutZ = -5f;
	public float cameraZoomSpeed = 0.7f;

	private bool isTriggered = false;
	private Rigidbody rigidbody;
	private BoxCollider collider;
	private Vector3 upPos;

	private GameObject[] players;

	void Start () {
		//We need to keep track of the players so that we can see when they move past the gate
		players = GameObject.FindGameObjectsWithTag ("Player");

		rigidbody = GetComponent<Rigidbody> ();
		collider = GetComponent<BoxCollider> ();

		//Keep track of the original position as we want to come back here when we raise the fence
		upPos = transform.position;

		//Don't detect collisions and sink to the lowest depth (hide the fence initially)
		//rigidbody.detectCollisions = false;
		collider.isTrigger = true;
		Vector3 pos = transform.position;
		pos.y -= depth;
		transform.position = pos;

	}

	void Update () {
		if (isTriggered){
			//if are triggered and aren't yet at the up position
			if (transform.position.y < upPos.y) {
				Vector3 pos = transform.position;
				pos.y += (raiseSpeed * Time.deltaTime);
				transform.position = pos;
			}

			if (Camera.main.transform.position.z > cameraZoomedOutZ){
				Vector3 camPos = Camera.main.transform.position;
				camPos.z = Mathf.Lerp(camPos.z, cameraZoomedOutZ, cameraZoomSpeed * Time.deltaTime);
				Camera.main.transform.position = camPos;
			}
		}
	}

	//Tells the fence to raise itself 
	//To be called when the players are both past the fence
	//Just sets the isTriggered variable to true and tells the
	//collider not to let the players pass through
	public void Trigger (){
		//rigidbody.detectCollisions = true;
		collider.isTrigger = false;
		isTriggered = true;
	}


	void OnTriggerExit(Collider other) {
		if(other.tag == "Player" && !isTriggered){
			Debug.Log ("PlayerTrigger " + other.name + " "+ AllPlayersPassed());
			if (AllPlayersPassed ()) {
				Trigger();
			}
		}
	}

	private bool AllPlayersPassed () {
		foreach (GameObject player in players) {
			float distToPlayer = player.transform.position.x - transform.position.x;
			if (distToPlayer < rightTriggerDist) {
				return false;
			}
		}
		
		return true;
	}
}
