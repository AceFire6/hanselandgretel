/*
 * Controls for the movement of the arrow. It moves the arrow in the direction it spawned until it collides, at which
 * point it will attach itself to the object it hit.
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

		//Destroys the gameobject when it leaves the screen.
		//If in scene view, it will only destroy when it leaves the scene view screen.
		//Could also keep track of how far the arrow has gone and destroy it based on that
			//but the then it depends on the camera zoom
		if (!renderer.isVisible) 
		{
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{		//When it hits something other than the player
		GameObject obj = collision.gameObject;
		if (obj.tag != "Player" && obj.name != "BBWolf_Unity" && obj.name != "BH_Unity") {
			body.isKinematic = true; 	//Prevent other forces from moving it
			position.x += 0.03f * direction;
			body.MovePosition (position); //Move the arrow a bit so the it looks like it cut into the object it struck
			collider.isTrigger = true; //Prevent it from colliding with other objects
			active = false;
			transform.parent = collision.transform;

			if (obj.tag == "Minion") //Do damage to minions
				obj.GetComponent<Health> ().TakeDamage (50);
		} else 
		{
			obj.GetComponent<Health> ().TakeDamage (50);
			Destroy (gameObject);
		}
	}
}
