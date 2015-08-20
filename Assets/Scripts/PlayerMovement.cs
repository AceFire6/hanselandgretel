using UnityEngine;
using System.Collections;

public class PlayerMovement : Movement
{

	Animator animator;
	Vector3 position;

	public float movementSpeed = 1f;
	public float strafeSpeed = 0.5f;
	public float rotationSpeed = 180f;
	public float jumpSpeed = 200f;
	bool canJump;

	void Start () 
	{
		base.Start ();
		base.speed = 1.0f;
		base.rotSpeed = 2.5f;
		canJump = true;
		animator = GetComponent<Animator> ();
		position = transform.position;
	}

	void OnCollisionEnter(Collision collision)
	{
		canJump = true;
		//Debug.Log ("Collision");
	}

	void OnCollisionExit(Collision collision)
	{
		canJump = false;
		//Debug.Log (" No Collision");

	}

	bool IsGrounded()
	{
		return Physics.Raycast (transform.position, -Vector3.up, collider.bounds.extents.y + 0.1f);
	}

	void Update()
	{
		base.Update ();

		//Debug.Log (animator.GetCurrentAnimatorStateInfo (0).normalizedTime);//.IsName("Base Layer.Idle"));
		float h = Input.GetAxisRaw ("Horizontal"); //Since we're only moving in this one plane
		//canJump = !animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Jump");
		//Debug.Log (canJump);

		bool running = h != 0;									//Strafing only if shift is pressed, player is running and moving backwards
		bool strafing = Input.GetKey (KeyCode.LeftShift) && running && transform.forward.x * h < 0;
		bool shooting = Input.GetKeyDown (KeyCode.Space);
		bool jumping = Input.GetKeyDown (KeyCode.W) && IsGrounded();// && canJump;

		//Debug.Log (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Jump"));

		Debug.Log(Physics.Raycast (transform.position, -Vector3.up, collider.bounds.extents.y + 0.1f));
		if (jumping) 
			objRigidbody.AddForce (transform.up * jumpSpeed);

		animator.SetBool ("isStrafing", strafing);
		animator.SetBool ("isShooting", shooting);
		animator.SetBool ("isJumping", jumping);
		animator.SetBool ("isRunning", running); 

		position.x = position.x + Time.deltaTime * movementSpeed * h; //Increment or decrement the players position 

		bool facingCorrect = (facing == Direction.Left && h > 0) || (facing == Direction.Right && h < 0);
		if (facingCorrect && !strafing)//Check if the player is moving in the direction it is facing
		{
			//If not, rotate the player to face the other direction
			if (facing == Direction.Left)
				RotateToFace(Direction.Right);
			else 
				RotateToFace(Direction.Left);
			//Can alternatively use lookAt as well, no 'smooth' rotation though.
			//transform.LookAt(transform.forward*-1); 
		}

		float v = 0;

		//if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.Jump") )//&& animator.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.5)
	//		v = 10;//objRigidbody.AddForce (transform.up*10);//

		SetDeltaMovement (h, v);
	}


	void FixedUpdate () 
	{
		
	}
}
