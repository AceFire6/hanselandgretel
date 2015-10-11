using UnityEngine;
using System.Collections;

public class LowerFence : MonoBehaviour {

	public float lowerSpeed = 0.7f; //speed at which the fence will rise
	public float depth = 0.5f; //How far to sink/rise

	private bool isTriggered = false;
	private Rigidbody rigidbody;
	private BoxCollider collider;
	private Vector3 downPos;

	public GameObject boss;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		collider = GetComponent<BoxCollider> ();

		//Keep track of the original position as we want todownPos back here when we raise the fence
		downPos = transform.position;
		downPos.y -= depth;

	}

	void Update () {
		if (boss == null){
			rigidbody.detectCollisions = false;

			//if are triggered and aren't yetdownPoshe up position
			if (transform.position.y > downPos.y) {
				Vector3 pos = transform.position;
				pos.y -= (lowerSpeed * Time.deltaTime);
				transform.position = pos;
			}
		}
	}
}
