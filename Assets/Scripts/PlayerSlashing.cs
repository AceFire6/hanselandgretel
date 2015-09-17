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

	private float cooldown;
	private float timer;
	private Animator animator;
	private bool canSlash;
	
	void Start () 
	{
		animator = GetComponentInParent<Animator> ();
		cooldown = 0.3f;
	}

	//The axe has hit something
	//If it's an enemy and hansel can attack, then decrease the enemy's health
	//If the gameobject doesn't have a health component, just ignore it.
	void OnCollisionEnter(Collision collision) {
		if (canSlash && collision.gameObject.tag != "Player") {
			Health health = collision.gameObject.GetComponent<Health> ();
			if (health != null) {
				health.TakeDamage (slashDamage);
			}
		}
	}
	
	void Update () 
	{
		timer += Time.deltaTime;

		bool slashing = animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.StandAndShootv2") || animator.GetCurrentAnimatorStateInfo (0).IsName ("Base Layer.RunAndShoot");

		if (timer >= cooldown && slashing){
			timer = 0.0f;
			canSlash = true;
		}
	}
}