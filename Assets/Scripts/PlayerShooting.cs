/*
 * Script to control spawning arrows whenever Gretel shoots. Based on the animation rather than the keyboard press
 * since the movement script handles the keyboard presses and it prevents firing when the player is jumping.
 * 
 * Author: Aashiq Parker
 * Date: 21-August-2015
 */

using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public Transform arrow;

	private float cooldown;
	private float timer;
	private Vector3 positionOffset; //Uses an offset to place the arrow since the bow moves up and down because of the
										//animation but the position doesn't change
	Animator animator;

	void Start () 
	{
		animator = GetComponent<Animator> ();
		positionOffset = new Vector3 (0, 0.55f, 0);
		cooldown = 0.292f * 2; //Time it takes to raise and lower bow
	}

	void Shoot()
	{
		positionOffset.x = transform.forward.x * 0.01f;
		Instantiate (arrow, transform.position + positionOffset,transform.rotation);
	}

	void Update () 
	{
		timer += Time.deltaTime;				//Check if the player performing the shooting animation.
		bool shooting = animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.RaiseCrossbow"); 
		if (timer > cooldown && shooting)
		{
			timer = 0;
			Shoot ();
		}

	}
}
