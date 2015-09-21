/* Handles the slashing of axes(Hansel). This script needs to be attached to the
 * axe/hand bone of Hansel's rig. This is so the collider (also attached to the rig)
 * will move with the animation allowing for the collisions detected to be accurate.
 * Damage is dealt when the weapon collider collides with an enemy while hansel is
 * attacking.
 * 
 * Author: Muhummad Patel
 * Date: 16-September-2015
 */
		
using UnityEngine;
using System.Collections;

public class PlayerSlashing : MonoBehaviour {
		
	public int slashDamage = 10;

	private Animator animator;
	private bool canSlash;
	
	void Start () 
	{
		animator = GetComponentInParent<Animator> ();
	}

	//The axe has hit something
	//If it's an enemy and hansel can attack, then decrease the enemy's health
	//If the gameobject doesn't have a health component, just ignore it.
	void OnCollisionStay(Collision collision) {
		if (canSlash && collision.gameObject.tag != "Player") {
			Health health = collision.gameObject.GetComponent<Health> ();
			if (health != null) {
				health.TakeDamage (slashDamage);
			}
		}
	}
	
	void Update () 
	{
		canSlash = animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.StandAndAttack") || animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.RunAndChop");
	}
}