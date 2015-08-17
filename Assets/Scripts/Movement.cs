/*
 * Movement base class allows easy moving and animating of kinematic GameObjects.
 * Subclass this class (e.g. MovementMinion) to specialise the movement
 * for a specific GameObject and to implement that GameObjects animation
 * logic.
 * 
 * Important functions (Use these to move the gameObject):
 * SetDeltaMovement(float x, float y)
 * RotateToFace(Direction direction)
 * 
 * 
 * Author: Muhummad Patel
 * Date: 17-August-2015
 */

using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

	public enum Direction
	{
		Left,
		Right
	};
	//direction the GameObject faces initially. This MUST be set correctly for the prefab.
	public Direction facing;

	public float speed = 3.0f;
	public float rotSpeed = 5.0f;

	protected Vector3 deltaMovement; //set to 0 at every update.
	protected Vector3 rotation; //keeps track of the rotation of the GameObject.

	protected Rigidbody objRigidbody;
	
	protected virtual void Start ()
	{
		objRigidbody = GetComponent<Rigidbody> ();
		rotation = objRigidbody.rotation.eulerAngles; //initial rotation of the rigidBody
	}

	protected virtual void Update ()
	{		
		UpdatePosition ();
		UpdateRotation ();
	}

	//Move in the direction specified by deltaMovement at speed.
	//Sets deltaMovement back to zero.
	private void UpdatePosition ()
	{
		deltaMovement = deltaMovement.normalized * speed * Time.deltaTime;
		
		objRigidbody.MovePosition (transform.position + deltaMovement);
		deltaMovement = Vector3.zero;
	}

	//Rotate the rigidBody at every update until its rotation is the rotation we want.
	//NOTE: the rotation vector is NOT reset to zero.
	private void UpdateRotation ()
	{
		if (objRigidbody.rotation.eulerAngles != rotation) {
			Quaternion rot = Quaternion.Lerp (objRigidbody.rotation, Quaternion.Euler (rotation), rotSpeed);
			objRigidbody.MoveRotation (rot);
		}
	}

	//Increments the rotation of the rigidbody by the given amounts in those axes.
	//Used mainly to clamp to the range [0, 360]
	private void IncrementRotation (float x, float y)
	{
		rotation += new Vector3 (x, y, 0.0f);

		if (rotation.x > 360) {
			rotation.x -= 360;
		}
		if (rotation.y > 360) {
			rotation.y -= 360;
		}
	}

	//Move the object.
	//+ve x -> moves object right
	//-ve x -> moves object left
	//+ve y -> moves object up
	//-ve y -> moves object down
	public void SetDeltaMovement (float x, float y)
	{
		deltaMovement = new Vector3 (x, y, 0.0f);
	}

	//Rotate the object to face either left or right.
	//Pass in either Movement.Direction.Left, or Movement.Directon.Right
	public void RotateToFace (Direction direction)
	{
		if (facing != direction) {
			IncrementRotation (0.0f, 180.0f);
			facing = direction;
		}
	}
}
