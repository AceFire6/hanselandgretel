using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	Animator animator;
	Rigidbody body;
	Vector3 position;

	public float movementSpeed = 1f;
	public float strafeSpeed = 0.5f;
	public float rotationSpeed = 180f;

	void Start () 
	{
		body = GetComponent<Rigidbody>();
		animator = GetComponent<Animator> ();
		position = transform.position;
	}

	void Update()
	{

		float h = Input.GetAxisRaw ("Horizontal"); //Since we're only moving in this one plane

		bool running = h != 0;									//Strafing only if shift is pressed, player is running and moving backwards
		bool strafing = Input.GetKey (KeyCode.LeftShift) && running && transform.forward.x * h > 0;
		bool shooting = Input.GetKeyDown (KeyCode.Space);

		animator.SetBool ("isStrafing", strafing);
		animator.SetBool ("isShooting", shooting);
		animator.SetBool ("isRunning", running); 

		position.x = position.x + Time.deltaTime * movementSpeed * -h; //Increment or decrement the players position 

		if (transform.forward.x * h > 0 && !strafing) //Check if the player is moving in the direction it is facing
		{
			//If not, rotate the player to face the other direction
			Quaternion rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( transform.forward*-1 ), rotationSpeed*Time.deltaTime );
			body.MoveRotation(rotation);

			//Can alternatively use lookAt as well, no 'smooth' rotation though.
			//transform.LookAt(transform.forward*-1); 
		}
		body.MovePosition (position);
	}


	void FixedUpdate () 
	{
	
	}
}
