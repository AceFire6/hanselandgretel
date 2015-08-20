using UnityEngine;
using System.Collections;

public class ArrowMovement : MonoBehaviour {

	public float speed = 5f;
	private bool active = true;
	private float direction;

	Rigidbody body;
	Vector3 position;

	void Start () 
	{
		body = GetComponent<Rigidbody> ();
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
		Debug.Log ("Collision");
		body.isKinematic = true;
		active = false;
		transform.parent = collision.transform;
	}
}
