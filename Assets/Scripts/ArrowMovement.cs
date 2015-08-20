using UnityEngine;
using System.Collections;

public class ArrowMovement : MonoBehaviour {

	public float speed = 5f;
	private bool active = true;
	private float direction;

	Rigidbody body;
	Vector3 position;
	Collider collider;

	void Start () 
	{
		body = GetComponent<Rigidbody> ();
		collider = GetComponent<Collider> ();
		position = transform.position;
		direction = transform.forward.x; 
	}

	void Update () 
	{
		if (active) 
		{	
			position.x += speed * Time.deltaTime * direction;
			body.MovePosition (position);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Player")
		{
			Debug.Log ("Collision");
			body.isKinematic = true;
			position.x += 0.1f * direction;
			body.MovePosition (position); //Move the arrow a bit so the it looks like it cut into the object it struck
			collider.isTrigger = true;
			active = false;
			transform.parent = collision.transform;
		}
	}
}
