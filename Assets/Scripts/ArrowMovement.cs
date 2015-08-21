/*
 * Controls for the movement of the arrow. It moves the bow in the direction it spawned until it collides, at which
 * it will attach itself to the object it hit.
 * 
 * Author: Aashiq Parker
 * Date: 21-August-2015
 */

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
	{		//When it hits something other than the player
		if (collision.gameObject.tag != "Player")  
		{
			Debug.Log ("Collision");
			body.isKinematic = true; 	//Prevent other forces from moving it
			position.x += 0.05f * direction;
			body.MovePosition (position); //Move the arrow a bit so the it looks like it cut into the object it struck
			collider.isTrigger = true; //Prevent it from colliding with other objects
			active = false;
			transform.parent = collision.transform;
		}
	}
}
