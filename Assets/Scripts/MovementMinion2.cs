/*
 * Subclass of the Movement class that adds animation logic specific
 * to the minion model. 
 * 
 * Author: Muhummad Patel
 * Date: 17-August-2015
 */

using UnityEngine;
using System.Collections;

public class MovementMinion2 : Movement
{
	
	private Animator objAnimator; //animator controller component of this GameObject
	private bool isAttacking = false;

	protected override void Start ()
	{
		base.Start ();
		objAnimator = GetComponent<Animator> ();
	}

	protected override void Update ()
	{
		base.Update ();
		UpdateAnimation ();
	}

	public void SetIsAttacking (bool a) {
		isAttacking = a;
	}

	public bool getIsAttacking () {
		return isAttacking;
	}

	void UpdateAnimation ()
	{
		objAnimator.SetBool ("isFlying", deltaMovement != Vector3.zero);
		objAnimator.SetBool ("isAttacking", isAttacking);
	}
}
