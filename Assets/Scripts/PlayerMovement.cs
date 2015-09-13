/*
 * Script to control Gretel's movement. Inherits from Muhammad's base Movement Script.
 * Control are: 
 * 	'A' and 'D' or the arrow keys to move.
 * 	'W' to jump.
 * 	Left Shift + Move to strafe.
 * 	Spacebar to shoot.
 * 
 * Author: Aashiq Parker
 * Date: 20-August-2015
 */
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
	public float jumpOffset = 0.3f; //Helps to prevent double jumps, explained below
	public float timer = 0f;

	void Start () 
	{
		base.Start ();
		base.speed = 1.0f;
		base.rotSpeed = 2.5f;
		animator = GetComponent<Animator> ();
		position = transform.position;
	}
	
	bool IsGrounded() //To check if the player is on the ground so that he can jump again
	{															//Slight offset for raycast to work	
		return Physics.Raycast (transform.position + new Vector3(0,0.03f,0), -Vector3.up, collider.bounds.extents.y );
	}

	void Update()
	{
		base.Update ();
		timer += Time.deltaTime;
		float h = Input.GetAxisRaw ("Horizontal"); //Since we're only moving in this one plane

		bool running = h != 0;									//Strafing only if shift is pressed, player is running and moving backwards
		bool strafing = Input.GetKey (KeyCode.LeftShift) && running && transform.forward.x * h < 0;
		bool shooting = Input.GetKey (KeyCode.Space);
		bool jumping = Input.GetKeyDown (KeyCode.W) && IsGrounded();// && canJump;

		/* I initially tried using OnCollisionEnter and Exit to set a canJump variable to check if the player was
		 * on the ground before jumping again to prevent double jumps, but the collisions sometimes don't pickup. I
		 * suspect it's because the y-pos of the player fluctuates very slightly when walking (why I don't know).
		 * I then tried using a raycast, which initially didn't work but then I added a slight offset to the y position
		 * of the player and it did, the problem was that if the player pressed 'w' to jump fast enough then the player
		 * can still double jump. So what I finally did was at a slight offset and used a timer to disable jumping within
		 * the first 0.3 seconds of the jump, after which the raycast will fail beacuse the player is in the air.
		 * */
		if (jumping && timer > jumpOffset) 
		{
			objRigidbody.AddForce (transform.up * jumpSpeed);
			timer = 0f;
		}


		animator.SetBool ("isStrafing", strafing);
		animator.SetBool ("isJumping", jumping);
		animator.SetBool ("isRunning", running); 
		animator.SetBool ("isShooting", shooting && !(running || jumping));

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

		SetDeltaMovement (h, 0);
	}
}
